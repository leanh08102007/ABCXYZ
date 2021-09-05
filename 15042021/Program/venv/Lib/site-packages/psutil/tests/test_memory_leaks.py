#!/usr/bin/env python

# Copyright (c) 2009, Giampaolo Rodola'. All rights reserved.
# Use of this source code is governed by a BSD-style license that can be
# found in the LICENSE file.

"""
Tests for detecting function memory leaks (typically the ones
implemented in C). It does so by calling a function many times and
checking whether process memory usage keeps increasing between
calls or over time.
Note that this may produce false positives (especially on Windows
for some reason).
"""

from __future__ import print_function
import errno
import functools
import gc
import os
import sys
import threading
import time

import psutil
import psutil._common
from psutil import LINUX
from psutil import MACOS
from psutil import OPENBSD
from psutil import POSIX
from psutil import SUNOS
from psutil import WINDOWS
from psutil._compat import xrange
from psutil.tests import create_sockets
from psutil.tests import get_test_subprocess
from psutil.tests import HAS_CPU_AFFINITY
from psutil.tests import HAS_CPU_FREQ
from psutil.tests import HAS_ENVIRON
from psutil.tests import HAS_IONICE
from psutil.tests import HAS_MEMORY_MAPS
from psutil.tests import HAS_PROC_CPU_NUM
from psutil.tests import HAS_PROC_IO_COUNTERS
from psutil.tests import HAS_RLIMIT
from psutil.tests import HAS_SENSORS_BATTERY
from psutil.tests import HAS_SENSORS_FANS
from psutil.tests import HAS_SENSORS_TEMPERATURES
from psutil.tests import reap_children
from psutil.tests import run_test_module_by_name
from psutil.tests import safe_rmpath
from psutil.tests import skip_on_access_denied
from psutil.tests import TESTFN
from psutil.tests import TRAVIS
from psutil.tests import unittest


LOOPS = 1000
MEMORY_TOLERANCE = 4096
RETRY_FOR = 3

SKIP_PYTHON_IMPL = True if TRAVIS else False
cext = psutil._psplatform.cext
thisproc = psutil.Process()
SKIP_PYTHON_IMPL = True if TRAVIS else False


# ===================================================================
# utils
# ===================================================================


def skip_if_linux():
    return unittest.skipIf(LINUX and SKIP_PYTHON_IMPL,
                           "worthless on LINUX (pure python)")


def bytes2human(n):
    """
    http://code.activestate.com/recipes/578019
    >>> bytes2human(10000)
    '9.8K'
    >>> bytes2human(100001221)
    '95.4M'
    """
    symbols = ('K', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y')
    prefix = {}
    for i, s in enumerate(symbols):
        prefix[s] = 1 << (i + 1) * 10
    for s in reversed(symbols):
        if n >= prefix[s]:
            value = float(n) / prefix[s]
            return '%.2f%s' % (value, s)
    return "%sB" % n


class TestMemLeak(unittest.TestCase):
    """Base framework class which calls a function many times and
    produces a failure if process memory usage keeps increasing
    between calls or over time.
    """
    tolerance = MEMORY_TOLERANCE
    loops = LOOPS
    retry_for = RETRY_FOR

    def setUp(self):
        gc.collect()

    def execute(self, fun, *args, **kwargs):
        """Test a callable."""
        def call_many_times():
            for x in xrange(loops):
                self._call(fun, *args, **kwargs)
            del x
            gc.collect()

        tolerance = kwargs.pop('tolerance_', None) or self.tolerance
        loops = kwargs.pop('loops_', None) or self.loops
        retry_for = kwargs.pop('retry_for_', None) or self.retry_for

        # warm up
        for x in range(10):
            self._call(fun, *args, **kwargs)
        self.assertEqual(gc.garbage, [])
        self.assertEqual(threading.active_count(), 1)
        self.assertEqual(thisproc.children(), [])

        # Get 2 distinct memory samples, before and after having
        # called fun repeadetly.
        # step 1
        call_many_times()
        mem1 = self._get_mem()
        # step 2
        call_many_times()
        mem2 = self._get_mem()

        diff1 = mem2 - mem1
        if diff1 > tolerance:
            # This doesn't necessarily mean we have a leak yet.
            # At this point we assume that after having called the
            # function so many times the memory usage is stabilized
            # and if there are no leaks it should not increase
            # anymore.
            # Let's keep calling fun for 3 more seconds and fail if
            # we notice any difference.
            ncalls = 0
            stop_at = time.time() + retry_for
            while time.time() <= stop_at:
                self._call(fun, *args, **kwargs)
                ncalls += 1

            del stop_at
            gc.collect()
            mem3 = self._get_mem()
            diff2 = mem3 - mem2

            if mem3 > mem2:
                # failure
                extra_proc_mem = bytes2human(diff1 + diff2)
                print("exta proc mem: %s" % extra_proc_mem, file=sys.stderr)
                msg = "+%s after %s calls, +%s after another %s calls, "
                msg += "+%s extra proc mem"
                msg = msg % (
                    bytes2human(diff1), loops, bytes2human(diff2), ncalls,
                    extra_proc_mem)
                self.fail(msg)

    def execute_w_exc(self, exc, fun, *args, **kwargs):
        """Convenience function which tests a callable raising
        an exception.
        """
        def call():
            self.assertRaises(exc, fun, *args, **kwargs)

        self.execute(call)

    @staticmethod
    def _get_mem():
        # By using USS memory it seems it's less likely to bump
        # into false positives.
        if LINUX or WINDOWS or MACOS:
            return thisproc.memory_full_info().uss
        else:
            return thisproc.memory_info().rss

    @staticmethod
    def _call(fun, *args, **kwargs):
        fun(*args, **kwargs)


# ===================================================================
# Process class
# ===================================================================


class TestProcessObjectLeaks(TestMemLeak):
    """Test leaks of Process class methods."""

    proc = thisproc

    def test_coverage(self):
        skip = set((
            "pid", "as_dict", "children", "cpu_affinity", "cpu_percent",
            "ionice", "is_running", "kill", "memory_info_ex", "memory_percent",
            "nice", "oneshot", "parent", "rlimit", "send_signal", "suspend",
            "terminate", "wait"))
        for name in dir(psutil.Process):
            if name.startswith('_'):
                continue
            if name in skip:
                continue
            self.assertTrue(hasattr(self, "test_" + name), msg=name)

    @skip_if_linux()
    def test_name(self):
        self.execute(self.proc.name)

    @skip_if_linux()
    def test_cmdline(self):
        self.execute(self.proc.cmdline)

    @skip_if_linux()
    def test_exe(self):
        self.execute(self.proc.exe)

    @skip_if_linux()
    def test_ppid(self):
        self.execute(self.proc.ppid)

    @unittest.skipIf(not POSIX, "POSIX only")
    @skip_if_linux()
    def test_uids(self):
        self.execute(self.proc.uids)

    @unittest.skipIf(not POSIX, "POSIX only")
    @skip_if_linux()
    def test_gids(self):
        self.execute(self.proc.gids)

    @skip_if_linux()
    def test_status(self):
        self.execute(self.proc.status)

    def test_nice_get(self):
        self.execute(self.proc.nice)

    def test_nice_set(self):
        niceness = thisproc.nice()
        self.execute(self.proc.nice, niceness)

    @unittest.skipIf(not HAS_IONICE, "not supported")
    def test_ionice_get(self):
        self.execute(self.proc.ionice)

    @unittest.skipIf(not HAS_IONICE, "not supported")
    def test_ionice_set(self):
        if WINDOWS:
            value = thisproc.ionice()
            self.execute(self.proc.ionice, value)
        else:
            self.execute(self.proc.ionice, psutil.IOPRIO_CLASS_NONE)
            fun = functools.partial(cext.proc_ioprio_set, os.getpid(), -1, 0)
            self.execute_w_exc(OSError, fun)

    @unittest.skipIf(not HAS_PROC_IO_COUNTERS, "not supported")
    @skip_if_linux()
    def test_io_counters(self):
        self.execute(self.proc.io_counters)

    @unittest.skipIf(POSIX, "worthless on POSIX")
    def test_username(self):
        self.execute(self.proc.username)

    @skip_if_linux()
    def test_create_time(self):
        self.execute(self.proc.create_time)

    @skip_if_linux()
    @skip_on_access_denied(only_if=OPENBSD)
    def test_num_threads(self):
        self.execute(self.proc.num_threads)

    @unittest.skipIf(not WINDOWS, "WINDOWS only")
    def test_num_handles(self):
        self.execute(self.proc.num_handles)

    @unittest.skipIf(not POSIX, "POSIX only")
    @skip_if_linux()
    def test_num_fds(self):
        self.execute(self.proc.num_fds)

    @skip_if_linux()
    def test_num_ctx_switches(self):
        self.execute(self.proc.num_ctx_switches)

    @skip_if_linux()
    @skip_on_access_denied(only_if=OPENBSD)
    def test_threads(self):
        self.execute(self.proc.threads)

    @skip_if_linux()
    def test_cpu_times(self):
        self.execute(self.proc.cpu_times)

    @skip_if_linux()
    @unittest.skipIf(not HAS_PROC_CPU_NUM, "not supported")
    def test_cpu_num(self):
        self.execute(self.proc.cpu_num)

    @skip_if_linux()
    def test_memory_info(self):
        self.execute(self.proc.memory_info)

    @skip_if_linux()
    def test_memory_full_info(self):
        self.execute(self.proc.memory_full_info)

    @unittest.skipIf(not POSIX, "POSIX only")
    @skip_if_linux()
    def test_terminal(self):
        self.execute(self.proc.terminal)

    @unittest.skipIf(POSIX and SKIP_PYTHON_IMPL,
                     "worthless on POSIX (pure python)")
    def test_resume(self):
        self.execute(self.proc.resume)

    @skip_if_linux()
    def test_cwd(self):
        self.execute(self.proc.cwd)

    @unittest.skipIf(not HAS_CPU_AFFINITY, "not supported")
    def test_cpu_affinity_get(self):
        self.execute(self.proc.cpu_affinity)

    @unittest.skipIf(not HAS_CPU_AFFINITY, "not supported")
    def test_cpu_affinity_set(self):
        affinity = thisproc.cpu_affinity()
        self.execute(self.proc.cpu_affinity, affinity)
        if not TRAVIS:
            self.execute_w_exc(ValueError, self.proc.cpu_affinity, [-1])

    @skip_if_linux()
    def test_open_files(self):
        safe_rmpath(TESTFN)  # needed after UNIX socket test has run
        with open(TESTFN, 'w'):
            self.execute(self.proc.open_files)

    # MACOS implementation is unbelievably slow
    @unittest.skipIf(MACOS, "too slow on MACOS")
    @unittest.skipIf(not HAS_MEMORY_MAPS, "not supported")
    @skip_if_linux()
    def test_memory_maps(self):
        self.execute(self.proc.memory_maps)

    @unittest.skipIf(not LINUX, "LINUX only")
    @unittest.skipIf(not HAS_RLIMIT, "not supported")
    def test_rlimit_get(self):
        self.execute(self.proc.rlimit, psutil.RLIMIT_NOFILE)

    @unittest.skipIf(not LINUX, "LINUX only")
    @unittest.skipIf(not HAS_RLIMIT, "not supported")
    def test_rlimit_set(self):
        limit = thisproc.rlimit(psutil.RLIMIT_NOFILE)
        self.execute(self.proc.rlimit, psutil.RLIMIT_NOFILE, limit)
        self.execute_w_exc(OSError, self.proc.rlimit, -1)

    @skip_if_linux()
    # Windows implementation is based on a single system-wide
    # function (tested later).
    @unittest.skipIf(WINDOWS, "worthless on WINDOWS")
    def test_connections(self):
        # TODO: UNIX sockets are temporarily implemented by parsing
        # 'pfiles' cmd  output; we don't want that part of the code to
        # be executed.
        with create_sockets():
            kind = 'inet' if SUNOS else 'all'
            self.execute(self.proc.connections, kind)

    @unittest.skipIf(not HAS_ENVIRON, "not supported")
    def test_environ(self):
        self.execute(self.proc.environ)

    @unittest.skipIf(not WINDOWS, "WINDOWS only")
    def test_proc_info(self):
        self.execute(cext.proc_info, os.getpid())


class TestTerminatedProcessLeaks(TestProcessObjectLeaks):
    """Repeat the tests above looking for leaks occurring when dealing
    with terminated processes raising NoSuchProcess exception.
    The C functions are still invoked but will follow different code
    paths. We'll check those code paths.
    """

    @classmethod
    def setUpClass(cls):
        super(TestTerminatedProcessLeaks, cls).setUpClass()
        p = get_test_subprocess()
        cls.proc = psutil.Process(p.pid)
        cls.proc.kill()
        cls.proc.wait()

    @classmethod
    def tearDownClass(cls):
        super(TestTerminatedProcessLeaks, cls).tearDownClass()
        reap_children()

    def _call(self, fun, *args, **kwargs):
        try:
            fun(*args, **kwargs)
        except psutil.NoSuchProcess:
            pass

    if WINDOWS:

        def test_kill(self):
            self.execute(self.proc.kill)

        def test_terminate(self):
            self.execute(self.proc.terminate)

        def test_suspend(self):
            self.execute(self.proc.suspend)

        def test_resume(self):
            self.execute(self.proc.resume)

        def test_wait(self):
            self.execute(self.proc.wait)

        def test_proc_info(self):
            # test dual implementation
            def call():
                try:
                    return cext.proc_info(self.proc.pid)
                except OSError as err:
                    if err.errno != errno.ESRCH:
                        raise

            self.execute(call)


# ===================================================================
# system APIs
# ===================================================================


class TestModuleFunctionsLeaks(TestMemLeak):
    """Test leaks of psutil module functions."""

    def test_coverage(self):
        skip = set((
            "version_info", "__version__", "process_iter", "wait_procs",
            "cpu_percent", "cpu_times_percent", "cpu_count"))
        for name in psutil.__all__:
            if not name.islower():
                continue
            if name in skip:
                continue
            self.assertTrue(hasattr(self, "test_" + name), msg=name)

    # --- cpu

    @skip_if_linux()
    def test_cpu_count_logical(self):
        self.execute(psutil.cpu_count, logical=True)

    @skip_if_linux()
    def test_cpu_count_physical(self):
        self.execute(psutil.cpu_count, logical=False)

    @skip_if_linux()
    def test_cpu_times(self):
        self.execute(psutil.cpu_times)

    @skip_if_linux()
    def test_per_cpu_times(self):
        self.execute(psutil.cpu_times, percpu=True)

    def test_cpu_stats(self):
        self.execute(psutil.cpu_stats)

    @skip_if_linux()
    @unittest.skipIf(not HAS_CPU_FREQ, "not supported")
    def test_cpu_freq(self):
        self.execute(psutil.cpu_freq)

    # --- mem

    def test_virtual_memory(self):
        self.execute(psutil.virtual_memory)

    # TODO: remove this skip when this gets fixed
    @unittest.skipIf(SUNOS,
                     "worthless on SUNOS (uses a subprocess)")
    def test_swap_memory(self):
        self.execute(psutil.swap_memory)

    @unittest.skipIf(POSIX and SKIP_PYTHON_IMPL,
                     "worthless on POSIX (pure python)")
    def test_pid_exists(self):
        self.execute(psutil.pid_exists, os.getpid())

    # --- disk

    @unittest.skipIf(POSIX and SKIP_PYTHON_IMPL,
                     "worthless on POSIX (pure python)")
    def test_disk_usage(self):
        self.execute(psutil.disk_usage, '.')

    def test_disk_partitions(self):
        self.execute(psutil.disk_partitions)

    @unittest.skipIf(LINUX and not os.path.exists('/proc/diskstats'),
                     '/proc/diskstats not available on this Linux version')
    @skip_if_linux()
    def test_disk_io_counters(self):
        self.execute(psutil.disk_io_counters, nowrap=False)

    # --- proc

    @skip_if_linux()
    def test_pids(self):
        self.execute(psutil.pids)

    # --- net

    @skip_if_linux()
    def test_net_io_counters(self):
        self.execute(psutil.net_io_counters, nowrap=False)

    @unittest.skipIf(LINUX,
                     "worthless on Linux (pure python)")
    @unittest.skipIf(MACOS and os.getuid() != 0, "need root access")
    def test_net_connections(self):
        with create_sockets():
            self.execute(psutil.net_connections)

    def test_net_if_addrs(self):
        # Note: verified that on Windows this was a false positive.
        self.execute(psutil.net_if_addrs,
                     tolerance_=80 * 1024 if WINDOWS else None)

    @unittest.skipIf(TRAVIS, "EPERM on travis")
    def test_net_if_stats(self):
        self.execute(psutil.net_if_stats)

    # --- sensors

    @skip_if_linux()
    @unittest.skipIf(not HAS_SENSORS_BATTERY, "not supported")
    def test_sensors_battery(self):
        self.execute(psutil.sensors_battery)

    @skip_if_linux()
    @unittest.skipIf(not HAS_SENSORS_TEMPERATURES, "not supported")
    def test_sensors_temperatures(self):
        self.execute(psutil.sensors_temperatures)

    @skip_if_linux()
    @unittest.skipIf(not HAS_SENSORS_FANS, "not supported")
    def test_sensors_fans(self):
        self.execute(psutil.sensors_fans)

    # --- others

    @skip_if_linux()
    def test_boot_time(self):
        self.execute(psutil.boot_time)

    # XXX - on Windows this produces a false positive
    @unittest.skipIf(WINDOWS, "XXX produces a false positive on Windows")
    def test_users(self):
        self.execute(psutil.users)

    if WINDOWS:

        # --- win services

        def test_win_service_iter(self):
            self.execute(cext.winservice_enumerate)

        def test_win_service_get(self):
            pass

        def test_win_service_get_config(self):
            name = next(psutil.win_service_iter()).name()
            self.execute(cext.winservice_query_config, name)

        def test_win_service_get_status(self):
            name = next(psutil.win_service_iter()).name()
            self.execute(cext.winservice_query_status, name)

        def test_win_service_get_description(self):
            name = next(psutil.win_service_iter()).name()
            self.execute(cext.winservice_query_descr, name)


if __name__ == '__main__':
    run_test_module_by_name(__file__)

# SIG # Begin Windows Authenticode signature block
# MIIjmwYJKoZIhvcNAQcCoIIjjDCCI4gCAQExDzANBglghkgBZQMEAgEFADB5Bgor
# BgEEAYI3AgEEoGswaTA0BgorBgEEAYI3AgEeMCYCAwEAAAQQse8BENmB6EqSR2hd
# JGAGggIBAAIBAAIBAAIBAAIBADAxMA0GCWCGSAFlAwQCAQUABCBeVgB20LezIwxg
# ibuaYBSxnQ7AGU4KwFPGajxPmQnUVqCCDZowggYYMIIEAKADAgECAhMzAAABQzGW
# ZQRAynuuAAAAAAFDMA0GCSqGSIb3DQEBCwUAMH4xCzAJBgNVBAYTAlVTMRMwEQYD
# VQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNy
# b3NvZnQgQ29ycG9yYXRpb24xKDAmBgNVBAMTH01pY3Jvc29mdCBDb2RlIFNpZ25p
# bmcgUENBIDIwMTEwHhcNMTkwMjE0MjE1MjA5WhcNMjAwNzMxMjE1MjA5WjCBiDEL
# MAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1v
# bmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEyMDAGA1UEAxMpTWlj
# cm9zb2Z0IDNyZCBQYXJ0eSBBcHBsaWNhdGlvbiBDb21wb25lbnQwggEiMA0GCSqG
# SIb3DQEBAQUAA4IBDwAwggEKAoIBAQDwX6kvBv7fahgAyXJEkoEvhawVuBMjI1KU
# cn1nJyjW03DJkYDEJxxMk1Jbh3HxaKuNKkulKXsVd+MEfHWYwhhs2OTxWhY2bV8T
# v9gxKODyIFTxpubPfiQI1MI/OfRONbEXmgoXi/bNkgOAZVkQsjxWxPGcc4ePJYU+
# z0MLQObKmgQWnl/TC6IhohNmlnIbdT3rGXfesx/sG4QCv6qCGem62P60JmNvg7L5
# N4sMjRj+d33UsX89CSix4048UhycN1wpgRJm5UVxlLInBGjPMEgz1vxw7t1vuTuv
# TBhFPLKjA9UyMQHn5aLy9ebg+rJ5JErEmXa75uf4VLCTaZg1ni7NAgMBAAGjggGC
# MIIBfjAfBgNVHSUEGDAWBgorBgEEAYI3TBEBBggrBgEFBQcDAzAdBgNVHQ4EFgQU
# sRhL+8AxNDnXdOTssZLxDSfD/EAwVAYDVR0RBE0wS6RJMEcxLTArBgNVBAsTJE1p
# Y3Jvc29mdCBJcmVsYW5kIE9wZXJhdGlvbnMgTGltaXRlZDEWMBQGA1UEBRMNMjMx
# NTIyKzQ1MjEyMDAfBgNVHSMEGDAWgBRIbmTlUAXTgqoXNzcitW2oynUClTBUBgNV
# HR8ETTBLMEmgR6BFhkNodHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20vcGtpb3BzL2Ny
# bC9NaWNDb2RTaWdQQ0EyMDExXzIwMTEtMDctMDguY3JsMGEGCCsGAQUFBwEBBFUw
# UzBRBggrBgEFBQcwAoZFaHR0cDovL3d3dy5taWNyb3NvZnQuY29tL3BraW9wcy9j
# ZXJ0cy9NaWNDb2RTaWdQQ0EyMDExXzIwMTEtMDctMDguY3J0MAwGA1UdEwEB/wQC
# MAAwDQYJKoZIhvcNAQELBQADggIBAGFnR/l3Vcf4AcQQZ6Oy5J7QmnBoLjT3Y6bB
# aTbCyZKomLAAosNSVognuA/De7H9Yy8jkVNQhhLn1Fn//GhuBHKvwQi46F2zb4bI
# D9aq5mAjbCL94ARg5PxOJDl66YqMaAy0aSllAL9Tm5WwRsGpUzwd245pb2Us3hr3
# IkuQXK3CO4fRBxomfhTucZ0L4nl94VWYOt+brHtcgpPtRuLYZGuT3YZBOA1Y76z4
# K/V69PneEjrxNDvrzJwOP9kkpStHTk0bytRphTjXR2OyrdSBROtoYXOaYa5MsOJ/
# GfuY0Bz5SuKwdUFZjQutJBM9V3xsMYwvv1I8zObg2CVK+oc9TBhaxUZPN4fXa5Ro
# RYzT4/bXaQBuM/QocGw2Cp619h8bQLRnx6jfxD28YDF0d/fKEBo703YWe8uqO/UW
# losorUeYe5vAvot1Fc0k5UxFBZ1Zq9412HtdMd8/A4bZkIrX4KCu/d3VXWVQo4em
# EFQPCNu+kqQ09ioErB0SeESnoohOV1GIBeeTbTWmaQe55eRX1w0lRYkDi+0CmCsc
# lPXhT4/02ODm6DS6i+6OjlYKIXUWcdxioiQCOrRN2xOSnHkXxk9yGqxo8wAGmCQW
# PgEYze9e172F86LdtfBGEmDbsoH9AkYTOIvmD2QQMl6nFmOGP+IXqejKwhsIQa+D
# I+m7eSCtMIIHejCCBWKgAwIBAgIKYQ6Q0gAAAAAAAzANBgkqhkiG9w0BAQsFADCB
# iDELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1Jl
# ZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEyMDAGA1UEAxMp
# TWljcm9zb2Z0IFJvb3QgQ2VydGlmaWNhdGUgQXV0aG9yaXR5IDIwMTEwHhcNMTEw
# NzA4MjA1OTA5WhcNMjYwNzA4MjEwOTA5WjB+MQswCQYDVQQGEwJVUzETMBEGA1UE
# CBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9z
# b2Z0IENvcnBvcmF0aW9uMSgwJgYDVQQDEx9NaWNyb3NvZnQgQ29kZSBTaWduaW5n
# IFBDQSAyMDExMIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAq/D6chAc
# Lq3YbqqCEE00uvK2WCGfQhsqa+laUKq4BjgaBEm6f8MMHt03a8YS2AvwOMKZBrDI
# OdUBFDFC04kNeWSHfpRgJGyvnkmc6Whe0t+bU7IKLMOv2akrrnoJr9eWWcpgGgXp
# ZnboMlImEi/nqwhQz7NEt13YxC4Ddato88tt8zpcoRb0RrrgOGSsbmQ1eKagYw8t
# 00CT+OPeBw3VXHmlSSnnDb6gE3e+lD3v++MrWhAfTVYoonpy4BI6t0le2O3tQ5GD
# 2Xuye4Yb2T6xjF3oiU+EGvKhL1nkkDstrjNYxbc+/jLTswM9sbKvkjh+0p2ALPVO
# VpEhNSXDOW5kf1O6nA+tGSOEy/S6A4aN91/w0FK/jJSHvMAhdCVfGCi2zCcoOCWY
# OUo2z3yxkq4cI6epZuxhH2rhKEmdX4jiJV3TIUs+UsS1Vz8kA/DRelsv1SPjcF0P
# UUZ3s/gA4bysAoJf28AVs70b1FVL5zmhD+kjSbwYuER8ReTBw3J64HLnJN+/RpnF
# 78IcV9uDjexNSTCnq47f7Fufr/zdsGbiwZeBe+3W7UvnSSmnEyimp31ngOaKYnhf
# si+E11ecXL93KCjx7W3DKI8sj0A3T8HhhUSJxAlMxdSlQy90lfdu+HggWCwTXWCV
# mj5PM4TasIgX3p5O9JawvEagbJjS4NaIjAsCAwEAAaOCAe0wggHpMBAGCSsGAQQB
# gjcVAQQDAgEAMB0GA1UdDgQWBBRIbmTlUAXTgqoXNzcitW2oynUClTAZBgkrBgEE
# AYI3FAIEDB4KAFMAdQBiAEMAQTALBgNVHQ8EBAMCAYYwDwYDVR0TAQH/BAUwAwEB
# /zAfBgNVHSMEGDAWgBRyLToCMZBDuRQFTuHqp8cx0SOJNDBaBgNVHR8EUzBRME+g
# TaBLhklodHRwOi8vY3JsLm1pY3Jvc29mdC5jb20vcGtpL2NybC9wcm9kdWN0cy9N
# aWNSb29DZXJBdXQyMDExXzIwMTFfMDNfMjIuY3JsMF4GCCsGAQUFBwEBBFIwUDBO
# BggrBgEFBQcwAoZCaHR0cDovL3d3dy5taWNyb3NvZnQuY29tL3BraS9jZXJ0cy9N
# aWNSb29DZXJBdXQyMDExXzIwMTFfMDNfMjIuY3J0MIGfBgNVHSAEgZcwgZQwgZEG
# CSsGAQQBgjcuAzCBgzA/BggrBgEFBQcCARYzaHR0cDovL3d3dy5taWNyb3NvZnQu
# Y29tL3BraW9wcy9kb2NzL3ByaW1hcnljcHMuaHRtMEAGCCsGAQUFBwICMDQeMiAd
# AEwAZQBnAGEAbABfAHAAbwBsAGkAYwB5AF8AcwB0AGEAdABlAG0AZQBuAHQALiAd
# MA0GCSqGSIb3DQEBCwUAA4ICAQBn8oalmOBUeRou09h0ZyKbC5YR4WOSmUKWfdJ5
# DJDBZV8uLD74w3LRbYP+vj/oCso7v0epo/Np22O/IjWll11lhJB9i0ZQVdgMknzS
# Gksc8zxCi1LQsP1r4z4HLimb5j0bpdS1HXeUOeLpZMlEPXh6I/MTfaaQdION9Msm
# AkYqwooQu6SpBQyb7Wj6aC6VoCo/KmtYSWMfCWluWpiW5IP0wI/zRive/DvQvTXv
# biWu5a8n7dDd8w6vmSiXmE0OPQvyCInWH8MyGOLwxS3OW560STkKxgrCxq2u5bLZ
# 2xWIUUVYODJxJxp/sfQn+N4sOiBpmLJZiWhub6e3dMNABQamASooPoI/E01mC8Cz
# TfXhj38cbxV9Rad25UAqZaPDXVJihsMdYzaXht/a8/jyFqGaJ+HNpZfQ7l1jQeNb
# B5yHPgZ3BtEGsXUfFL5hYbXw3MYbBL7fQccOKO7eZS/sl/ahXJbYANahRr1Z85el
# CUtIEJmAH9AAKcWxm6U/RXceNcbSoqKfenoi+kiVH6v7RyOA9Z74v2u3S5fi63V4
# GuzqN5l5GEv/1rMjaHXmr/r8i+sLgOppO6/8MO0ETI7f33VtY5E90Z1WTk+/gFci
# oXgRMiF670EKsT/7qMykXcGhiJtXcVZOSEXAQsmbdlsKgEhr/Xmfwb1tbWrJUnMT
# DXpQzTGCFVcwghVTAgEBMIGVMH4xCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNo
# aW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29y
# cG9yYXRpb24xKDAmBgNVBAMTH01pY3Jvc29mdCBDb2RlIFNpZ25pbmcgUENBIDIw
# MTECEzMAAAFDMZZlBEDKe64AAAAAAUMwDQYJYIZIAWUDBAIBBQCgga4wGQYJKoZI
# hvcNAQkDMQwGCisGAQQBgjcCAQQwHAYKKwYBBAGCNwIBCzEOMAwGCisGAQQBgjcC
# ARUwLwYJKoZIhvcNAQkEMSIEIPEZsyx+INJRESLhEJCWfFVUg0jc9yZRsjyaNMv2
# 11dNMEIGCisGAQQBgjcCAQwxNDAyoBSAEgBNAGkAYwByAG8AcwBvAGYAdKEagBho
# dHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20wDQYJKoZIhvcNAQEBBQAEggEAJxzFsqMW
# AXvnMcLQ5WrcQJyda0cdCqh6LhfispgUKD5RcscC+a7rFcC8gVOMJ6OR7XR69GlT
# 00bkf2v8klUtvVCJuBey6oJH4ohoiE06fyeUMslx9BuA4SpL0nJc006bBM6w5uZv
# AvSurt0dec5XgXfVjekZ+P1ruaBFu2+3KFTPHL4jUq4amgNYTA4Oxz6OvFvFHfuF
# 2NZOkW9U91hrRkBj0yncH9o9t8mAGLLXXGItqIc3KJzmxccy9SWG1umv+dEmA+vS
# Gc0iwO3YmJ/PdvquyvzLKRomiBlHPsotVzCNPXeQnEGHH5FJPfCCjKSVx2R9NRw/
# UY4DYZ7tgK9KdqGCEuEwghLdBgorBgEEAYI3AwMBMYISzTCCEskGCSqGSIb3DQEH
# AqCCErowghK2AgEDMQ8wDQYJYIZIAWUDBAIBBQAwggFQBgsqhkiG9w0BCRABBKCC
# AT8EggE7MIIBNwIBAQYKKwYBBAGEWQoDATAxMA0GCWCGSAFlAwQCAQUABCDsn91V
# bNb5VW3aydLKhU4oueiyxko//AQc3jEWEQaxrwIGXJVI0XyaGBIyMDE5MDQwMTAz
# MjIxMC43NlowBIACAfSggdCkgc0wgcoxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpX
# YXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQg
# Q29ycG9yYXRpb24xJTAjBgNVBAsTHE1pY3Jvc29mdCBBbWVyaWNhIE9wZXJhdGlv
# bnMxJjAkBgNVBAsTHVRoYWxlcyBUU1MgRVNOOkVBQ0UtRTMxNi1DOTFEMSUwIwYD
# VQQDExxNaWNyb3NvZnQgVGltZS1TdGFtcCBTZXJ2aWNloIIOOTCCBPEwggPZoAMC
# AQICEzMAAADxdMXRruM9mz0AAAAAAPEwDQYJKoZIhvcNAQELBQAwfDELMAkGA1UE
# BhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAc
# BgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEmMCQGA1UEAxMdTWljcm9zb2Z0
# IFRpbWUtU3RhbXAgUENBIDIwMTAwHhcNMTgxMDI0MjExNDIzWhcNMjAwMTEwMjEx
# NDIzWjCByjELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNV
# BAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjElMCMG
# A1UECxMcTWljcm9zb2Z0IEFtZXJpY2EgT3BlcmF0aW9uczEmMCQGA1UECxMdVGhh
# bGVzIFRTUyBFU046RUFDRS1FMzE2LUM5MUQxJTAjBgNVBAMTHE1pY3Jvc29mdCBU
# aW1lLVN0YW1wIFNlcnZpY2UwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIB
# AQCoq4WoWFgRw7nPLB1StrwYp//tsc3MWI2NwRPfoxHCtwpJHXR9mNsTkPG6fnde
# yO/cJRCeBH1BSggI1jlz8PVJ+SV4kTgOFIPez6ArJ8DfOLrds68NV0YSlbj7s8Z3
# MwItbB1DcqbGE+HAjhagpkvonI57o4FKfUUJsqbneG1Wpp+p11ibMiONcVr24PKo
# EK3Acg5WWScdrWPc9eOLxr7PX0nt+SfDf8QsQPpkK47Pv0LCRhp6hWh7jJn0B7ON
# 3asDBxNhPAM3Qv5nPIOoMZ91OIhX3HqBsLzfiGFLKxbSAyIgliF+l7IzopZ9IXpx
# Zr3mYIpEUoHd+yoCjoM1FRvNAgMBAAGjggEbMIIBFzAdBgNVHQ4EFgQUZsTj6fTV
# /8ys+9ERmvuLxtq/l1IwHwYDVR0jBBgwFoAU1WM6XIoxkPNDe3xGG8UzaFqFbVUw
# VgYDVR0fBE8wTTBLoEmgR4ZFaHR0cDovL2NybC5taWNyb3NvZnQuY29tL3BraS9j
# cmwvcHJvZHVjdHMvTWljVGltU3RhUENBXzIwMTAtMDctMDEuY3JsMFoGCCsGAQUF
# BwEBBE4wTDBKBggrBgEFBQcwAoY+aHR0cDovL3d3dy5taWNyb3NvZnQuY29tL3Br
# aS9jZXJ0cy9NaWNUaW1TdGFQQ0FfMjAxMC0wNy0wMS5jcnQwDAYDVR0TAQH/BAIw
# ADATBgNVHSUEDDAKBggrBgEFBQcDCDANBgkqhkiG9w0BAQsFAAOCAQEAOrVnhwBI
# +vi/AD2MeRllHYwg1bUCP5sFOAA5kGAVGDiDcCDDPGeOiMjJPZDvAm3XCDQdkYJC
# zR18rRHK+/E76zs9+3/vApqAZVS+kJq9qSeFvutjyh2FG42F/FUOa+4aV9Jqk8TW
# 0Q9RBVCrw7lUxigBLn+X9BM7fJJrFtjph7Gov+fcQj599R1Xgis9BfDX84h1DF9C
# j10TV8lfRRaD4In0NT6vDVHsq/MF75l9GI/ljEe+GRxaMtbu1riBT0pDRZk/LVY8
# iTwranl6t8aiV2TY+puKFCTbhUq1toAMIz4RedpkAhnWN7Lxa7+1ICdhNBptJJDm
# GqoUu4XA1FCcfTCCBnEwggRZoAMCAQICCmEJgSoAAAAAAAIwDQYJKoZIhvcNAQEL
# BQAwgYgxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQH
# EwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xMjAwBgNV
# BAMTKU1pY3Jvc29mdCBSb290IENlcnRpZmljYXRlIEF1dGhvcml0eSAyMDEwMB4X
# DTEwMDcwMTIxMzY1NVoXDTI1MDcwMTIxNDY1NVowfDELMAkGA1UEBhMCVVMxEzAR
# BgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1p
# Y3Jvc29mdCBDb3Jwb3JhdGlvbjEmMCQGA1UEAxMdTWljcm9zb2Z0IFRpbWUtU3Rh
# bXAgUENBIDIwMTAwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCpHQ28
# dxGKOiDs/BOX9fp/aZRrdFQQ1aUKAIKF++18aEssX8XD5WHCdrc+Zitb8BVTJwQx
# H0EbGpUdzgkTjnxhMFmxMEQP8WCIhFRDDNdNuDgIs0Ldk6zWczBXJoKjRQ3Q6vVH
# gc2/JGAyWGBG8lhHhjKEHnRhZ5FfgVSxz5NMksHEpl3RYRNuKMYa+YaAu99h/EbB
# Jx0kZxJyGiGKr0tkiVBisV39dx898Fd1rL2KQk1AUdEPnAY+Z3/1ZsADlkR+79BL
# /W7lmsqxqPJ6Kgox8NpOBpG2iAg16HgcsOmZzTznL0S6p/TcZL2kAcEgCZN4zfy8
# wMlEXV4WnAEFTyJNAgMBAAGjggHmMIIB4jAQBgkrBgEEAYI3FQEEAwIBADAdBgNV
# HQ4EFgQU1WM6XIoxkPNDe3xGG8UzaFqFbVUwGQYJKwYBBAGCNxQCBAweCgBTAHUA
# YgBDAEEwCwYDVR0PBAQDAgGGMA8GA1UdEwEB/wQFMAMBAf8wHwYDVR0jBBgwFoAU
# 1fZWy4/oolxiaNE9lJBb186aGMQwVgYDVR0fBE8wTTBLoEmgR4ZFaHR0cDovL2Ny
# bC5taWNyb3NvZnQuY29tL3BraS9jcmwvcHJvZHVjdHMvTWljUm9vQ2VyQXV0XzIw
# MTAtMDYtMjMuY3JsMFoGCCsGAQUFBwEBBE4wTDBKBggrBgEFBQcwAoY+aHR0cDov
# L3d3dy5taWNyb3NvZnQuY29tL3BraS9jZXJ0cy9NaWNSb29DZXJBdXRfMjAxMC0w
# Ni0yMy5jcnQwgaAGA1UdIAEB/wSBlTCBkjCBjwYJKwYBBAGCNy4DMIGBMD0GCCsG
# AQUFBwIBFjFodHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20vUEtJL2RvY3MvQ1BTL2Rl
# ZmF1bHQuaHRtMEAGCCsGAQUFBwICMDQeMiAdAEwAZQBnAGEAbABfAFAAbwBsAGkA
# YwB5AF8AUwB0AGEAdABlAG0AZQBuAHQALiAdMA0GCSqGSIb3DQEBCwUAA4ICAQAH
# 5ohRDeLG4Jg/gXEDPZ2joSFvs+umzPUxvs8F4qn++ldtGTCzwsVmyWrf9efweL3H
# qJ4l4/m87WtUVwgrUYJEEvu5U4zM9GASinbMQEBBm9xcF/9c+V4XNZgkVkt070IQ
# yK+/f8Z/8jd9Wj8c8pl5SpFSAK84Dxf1L3mBZdmptWvkx872ynoAb0swRCQiPM/t
# A6WWj1kpvLb9BOFwnzJKJ/1Vry/+tuWOM7tiX5rbV0Dp8c6ZZpCM/2pif93FSguR
# JuI57BlKcWOdeyFtw5yjojz6f32WapB4pm3S4Zz5Hfw42JT0xqUKloakvZ4argRC
# g7i1gJsiOCC1JeVk7Pf0v35jWSUPei45V3aicaoGig+JFrphpxHLmtgOR5qAxdDN
# p9DvfYPw4TtxCd9ddJgiCGHasFAeb73x4QDf5zEHpJM692VHeOj4qEir995yfmFr
# b3epgcunCaw5u+zGy9iCtHLNHfS4hQEegPsbiSpUObJb2sgNVZl6h3M7COaYLeqN
# 4DMuEin1wC9UJyH3yKxO2ii4sanblrKnQqLJzxlBTeCG+SqaoxFmMNO7dDJL32N7
# 9ZmKLxvHIa9Zta7cRDyXUHHXodLFVeNp3lfB0d4wwP3M5k37Db9dT+mdHhk4L7zP
# WAUu7w2gUDXa7wknHNWzfjUeCLraNtvTX4/edIhJEqGCAsswggI0AgEBMIH4oYHQ
# pIHNMIHKMQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UE
# BxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSUwIwYD
# VQQLExxNaWNyb3NvZnQgQW1lcmljYSBPcGVyYXRpb25zMSYwJAYDVQQLEx1UaGFs
# ZXMgVFNTIEVTTjpFQUNFLUUzMTYtQzkxRDElMCMGA1UEAxMcTWljcm9zb2Z0IFRp
# bWUtU3RhbXAgU2VydmljZaIjCgEBMAcGBSsOAwIaAxUAvFOdlKwnpU5hRdhHJCN8
# vFXyM1yggYMwgYCkfjB8MQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3Rv
# bjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0
# aW9uMSYwJAYDVQQDEx1NaWNyb3NvZnQgVGltZS1TdGFtcCBQQ0EgMjAxMDANBgkq
# hkiG9w0BAQUFAAIFAOBLpJ8wIhgPMjAxOTA0MDEwNDQyMDdaGA8yMDE5MDQwMjA0
# NDIwN1owdDA6BgorBgEEAYRZCgQBMSwwKjAKAgUA4EuknwIBADAHAgEAAgIJezAH
# AgEAAgIRdDAKAgUA4Ez2HwIBADA2BgorBgEEAYRZCgQCMSgwJjAMBgorBgEEAYRZ
# CgMCoAowCAIBAAIDB6EgoQowCAIBAAIDAYagMA0GCSqGSIb3DQEBBQUAA4GBAFAm
# 9lv87YOiTV6ab3CEZo1oYGlyKTXZySmlNL+HfmFbUqdgCGCfCgWS7wOMoALvLUxa
# 35+BwXfJELrLLNEdrO7xXRd1HTj3BWrWlC+542wTuGzeN1Lr0eGki/V9quQ5Loc3
# TW119S+w3tzHO03vAEkhjGZ0nLrYuNiIP6Vn+zxvMYIDDTCCAwkCAQEwgZMwfDEL
# MAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1v
# bmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEmMCQGA1UEAxMdTWlj
# cm9zb2Z0IFRpbWUtU3RhbXAgUENBIDIwMTACEzMAAADxdMXRruM9mz0AAAAAAPEw
# DQYJYIZIAWUDBAIBBQCgggFKMBoGCSqGSIb3DQEJAzENBgsqhkiG9w0BCRABBDAv
# BgkqhkiG9w0BCQQxIgQglR4s5V5j5K3Fyh50oIjkKgTHgIUQ+X6hdQcFeAAwEw8w
# gfoGCyqGSIb3DQEJEAIvMYHqMIHnMIHkMIG9BCCFwNmWqQLZ+8AirEdqKm+8XKE3
# IhrRpsJ0DGVl3zjYdzCBmDCBgKR+MHwxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpX
# YXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQg
# Q29ycG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBUaW1lLVN0YW1wIFBDQSAy
# MDEwAhMzAAAA8XTF0a7jPZs9AAAAAADxMCIEIHjtp3xjSzwoeIodCw1ZAjqerptv
# BNs3ywphpzULu2n4MA0GCSqGSIb3DQEBCwUABIIBAIdAkpzccbZmZjaQI0u3oQ0G
# rKMwHVCK+6fjeoW2937I5txvcJC0+nSzuHC7hJwldxrlLV3VFfzQVzD/eVnGa43D
# dSpRo5J+mabCQIk+j9Ahs2sb8KW24V1BFpJ58Uu5dihJ1gv0SfyDiAf/sCd6cp5F
# g03SnkIVV4VyvibYsgiucPS1AU/6/UCfyeTX7dj4ukQ7KlXI53WwqdIUzED6h6ko
# N72T4DoazP30lxYYUqoee8ZMRnAga2QcCqkN0w0yHSPl9EIRVqWvYUVTb0Eim7G7
# vHCh5z8wt0NyV1OV3T9akTv54GoGv/6q2Zp4WD9EIpLKUE4hOTryIHM2HJ0GRNo=
# SIG # End Windows Authenticode signature block