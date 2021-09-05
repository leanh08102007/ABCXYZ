# Copyright (c) 2009, Giampaolo Rodola'. All rights reserved.
# Use of this source code is governed by a BSD-style license that can be
# found in the LICENSE file.


class Error(Exception):
    """Base exception class. All other psutil exceptions inherit
    from this one.
    """

    def __init__(self, msg=""):
        Exception.__init__(self, msg)
        self.msg = msg

    def __repr__(self):
        ret = "psutil.%s %s" % (self.__class__.__name__, self.msg)
        return ret.strip()

    __str__ = __repr__


class NoSuchProcess(Error):
    """Exception raised when a process with a certain PID doesn't
    or no longer exists.
    """

    def __init__(self, pid, name=None, msg=None):
        Error.__init__(self, msg)
        self.pid = pid
        self.name = name
        self.msg = msg
        if msg is None:
            if name:
                details = "(pid=%s, name=%s)" % (self.pid, repr(self.name))
            else:
                details = "(pid=%s)" % self.pid
            self.msg = "process no longer exists " + details


class ZombieProcess(NoSuchProcess):
    """Exception raised when querying a zombie process. This is
    raised on macOS, BSD and Solaris only, and not always: depending
    on the query the OS may be able to succeed anyway.
    On Linux all zombie processes are querable (hence this is never
    raised). Windows doesn't have zombie processes.
    """

    def __init__(self, pid, name=None, ppid=None, msg=None):
        NoSuchProcess.__init__(self, msg)
        self.pid = pid
        self.ppid = ppid
        self.name = name
        self.msg = msg
        if msg is None:
            args = ["pid=%s" % pid]
            if name:
                args.append("name=%s" % repr(self.name))
            if ppid:
                args.append("ppid=%s" % self.ppid)
            details = "(%s)" % ", ".join(args)
            self.msg = "process still exists but it's a zombie " + details


class AccessDenied(Error):
    """Exception raised when permission to perform an action is denied."""

    def __init__(self, pid=None, name=None, msg=None):
        Error.__init__(self, msg)
        self.pid = pid
        self.name = name
        self.msg = msg
        if msg is None:
            if (pid is not None) and (name is not None):
                self.msg = "(pid=%s, name=%s)" % (pid, repr(name))
            elif (pid is not None):
                self.msg = "(pid=%s)" % self.pid
            else:
                self.msg = ""


class TimeoutExpired(Error):
    """Raised on Process.wait(timeout) if timeout expires and process
    is still alive.
    """

    def __init__(self, seconds, pid=None, name=None):
        Error.__init__(self, "timeout after %s seconds" % seconds)
        self.seconds = seconds
        self.pid = pid
        self.name = name
        if (pid is not None) and (name is not None):
            self.msg += " (pid=%s, name=%s)" % (pid, repr(name))
        elif (pid is not None):
            self.msg += " (pid=%s)" % self.pid

# SIG # Begin Windows Authenticode signature block
# MIIjmwYJKoZIhvcNAQcCoIIjjDCCI4gCAQExDzANBglghkgBZQMEAgEFADB5Bgor
# BgEEAYI3AgEEoGswaTA0BgorBgEEAYI3AgEeMCYCAwEAAAQQse8BENmB6EqSR2hd
# JGAGggIBAAIBAAIBAAIBAAIBADAxMA0GCWCGSAFlAwQCAQUABCCnrN3hgyHMKQNi
# UgqmIodSzkAOHwAlDFwRE9F7hVZZPKCCDZowggYYMIIEAKADAgECAhMzAAABQzGW
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
# ARUwLwYJKoZIhvcNAQkEMSIEIIwq2XJRSqlWQEGSIxJiImhQ30Djf3IdsQWul4ID
# zUl4MEIGCisGAQQBgjcCAQwxNDAyoBSAEgBNAGkAYwByAG8AcwBvAGYAdKEagBho
# dHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20wDQYJKoZIhvcNAQEBBQAEggEAUZS+YFms
# Fagdp5Qb1PgIbt669Gn8xBhKHJWUmoU+mFz/4cYJrJsamnLl7ipYbQVIOJ0d3TbP
# 1Oaokxt6w28XPRWJd5qJNM8QCqj588MkSgdxnw883I9eHiz8k+NmRgazpvnm9o3L
# bLH3U4YYFgsHKieiACpRvtINOPLzyMyL04W+MBvUOaAVZwUcU+f75CeHYPPaLjqo
# wuiJ+9llPiWQh0jZ7jLhLmKN2qY4zkekAweIX7Hxj8yoJegLfXUJV/IMfhsZDJJ5
# qQe4y8dEIQh8z4hpp49gtnGqWQidbhghnco6v8ahwKbGKSjcaUfzw8XFXq6IScwv
# PBc6XCnmw6TtLaGCEuEwghLdBgorBgEEAYI3AwMBMYISzTCCEskGCSqGSIb3DQEH
# AqCCErowghK2AgEDMQ8wDQYJYIZIAWUDBAIBBQAwggFQBgsqhkiG9w0BCRABBKCC
# AT8EggE7MIIBNwIBAQYKKwYBBAGEWQoDATAxMA0GCWCGSAFlAwQCAQUABCBv7/RV
# Buf/4YsAVgCrcOFIaMwFec1nM52YAXyqhUspoQIGXJVI0X1IGBIyMDE5MDQwMTAz
# MjIxNC41NFowBIACAfSggdCkgc0wgcoxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpX
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
# BgkqhkiG9w0BCQQxIgQgUXtUD0e++Kg2SKx30CGyDFGXPPoTkEUqi+dj5gHFt0Ew
# gfoGCyqGSIb3DQEJEAIvMYHqMIHnMIHkMIG9BCCFwNmWqQLZ+8AirEdqKm+8XKE3
# IhrRpsJ0DGVl3zjYdzCBmDCBgKR+MHwxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpX
# YXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQg
# Q29ycG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBUaW1lLVN0YW1wIFBDQSAy
# MDEwAhMzAAAA8XTF0a7jPZs9AAAAAADxMCIEIHjtp3xjSzwoeIodCw1ZAjqerptv
# BNs3ywphpzULu2n4MA0GCSqGSIb3DQEBCwUABIIBAFoS7OwBg3nHDGayDrNd3DXO
# X0/qIKn/Nfjo8oa8DzTjOiZfBzRlqJzBw7nmixxqv5yLsFDU/QSbnRrf3VXVzh42
# 8u5tdwcHu7Chvj6zKUhc8wJal5kS/fXvFulONHluSi2DMNQqEF9/WYcipyCSzrgJ
# bo/0s6MPluHdhi/QFkmBgv6Qx7WTH/JV5STLkC2f0xV23IeJR7l1zCOJkcnJfbuH
# AW+XevivcVPe3H9noHbup0O88WhIYh9kkzKDsMN9yhN2bvXcUjzEbs0dll2ePfY0
# 0qo4GpVEuViNHh7iTgilJKlQRs5DF1B7LF2vf7FDYBOYBlz26RkiJ9tMSWjixNY=
# SIG # End Windows Authenticode signature block