using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using CommonHelper;
using System.Data;
using System.Data.OracleClient;
using websevice_lamvtneww_v2;


/// <summary>
/// Summary description for readlog_home
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class readlog_home : System.Web.Services.WebService
{
    OracleConnection conn2 = null;
    OracleConnection conn = null;
    public readlog_home()
    {
        //
        // TODO: Add constructor logic here
        //
        try
        {
            conn = new OracleConnection();
            conn.ConnectionString = KetNoi.str;
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }


            conn2 = new OracleConnection();
            conn2.ConnectionString = KetNoi.str2;
            if (conn2.State == ConnectionState.Open)
            {
                conn2.Close();
            }
        }
        catch (Exception)
        {

        }
    }








    [WebMethod]
    public string smsnt_otp(string sdt, int noidung_otp, string taikhoan)
    {
        string kqua = "";
        int i = ktra_otp(taikhoan, sdt, noidung_otp);
        if (i == 1)
        {
            SMSNT.SMSQC nt = new SMSNT.SMSQC();
            string noidung = "Mã OTP của bạn là : " + noidung_otp;
            nt.SendMultiSMS(sdt, noidung, "HYN2", "abc123", 1);
            kqua = "OK";
        }
        if (i == 0)
        {
            kqua = "NOT OK";
        }
        return kqua;
    }

    [WebMethod]
    public void Ma_OTP(string sdt, string noidung_otp)
    {
        SMSNT.SMSQC nt = new SMSNT.SMSQC();
        nt.SendMultiSMS(sdt, noidung_otp, "HYN2", "abc123", 1);
    }

    [WebMethod]
    public string smsnt_ndung(string sdt, string noidung_otp, string taikhoan)
    {
        string kqua = "";
        int i = ktra_ndung(taikhoan, sdt, noidung_otp);
        if (i == 1)
        {
            SMSNT.SMSQC nt = new SMSNT.SMSQC();
            string noidung = "TTKD VNPT-HY:" + noidung_otp;
            nt.SendMultiSMS(sdt, noidung, "HYN2", "abc123", 1);
            kqua = "OK";
        }
        if (i == 0)
        {
            kqua = "NOT OK";
        }
        return kqua;
    }

    [WebMethod]
    public string WSThemND(string taikhoan, string ten, string sdt)
    {
        string kqua = "";
        int i = ThemND(taikhoan, ten, sdt);
        if (i == 1)
        {
            kqua = "OK";
        }
        if (i == 0)
        {
            kqua = "NOT OK";
        }
        return kqua;
    }
    [WebMethod]
    public string WSXoaND(string taikhoan)
    {
        string kqua = "";
        int i = XoaND(taikhoan);
        if (i == 1)
        {
            kqua = "OK";
        }
        if (i == 0)
        {
            kqua = "NOT OK";
        }
        return kqua;
    }
    [WebMethod]
    public string WSSuaND(string taikhoan, string ten, string sdt)
    {
        string kqua = "";
        int j = ktra_suand(taikhoan);
        if (j == 1)
        {
            int i = SuaND(taikhoan, ten, sdt);
            if (i == 1)
            {
                kqua = "OK";
            }
            if (i == 0)
            {
                kqua = "NOT OK";
            }
        }
        if (j == 0)
        {
            kqua = "NOT OK";
        }
        return kqua;
    }




    public int ktra_otp(string taikhoan, string sdt, int otp)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }

        string sql = "select * from nguoidung_ttkd where taikhoan = '" + taikhoan + "' and sdt = '" + sdt + "' ";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        if (cmd.ExecuteReader().HasRows)
        {
            sql = "update  nguoidung_ttkd set otp = '" + otp + "' where taikhoan = '" + taikhoan + "' and sdt = '" + sdt + "'";
            cmd = new OracleCommand(sql, conn2);
            cmd.ExecuteNonQuery();
            conn2.Close();
            return 1;//da co
        }
        else
        {
            conn2.Close();
            return 0;//chua co
        }
    }


    public int ktra_ndung(string taikhoan, string sdt, string noidung)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }

        string sql = "select * from nguoidung_ttkd where taikhoan = '" + taikhoan + "' and sdt = '" + sdt + "' ";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        if (cmd.ExecuteReader().HasRows)
        {
            Random rd = new Random();
            sql = "update  nguoidung_ttkd set noidung = '" + noidung + "' where  taikhoan = '" + taikhoan + "' and sdt = '" + sdt + "'   ";
            cmd = new OracleCommand(sql, conn2);
            cmd.ExecuteNonQuery();
            conn2.Close();
            return 1;//da co
        }
        else
        {
            conn2.Close();
            return 0;//chua co
        }
    }







    public int ThemND(string taikhoan, string ten, string sdt)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }

        try
        {
            string sql = "insert into  nguoidung_ttkd values ('" + taikhoan + "','" + ten + "','" + sdt + "','','')";
            OracleCommand cmd = new OracleCommand(sql, conn2);
            cmd = new OracleCommand(sql, conn2);
            cmd.ExecuteNonQuery();
            conn2.Close();
            return 1;//ok
        }
        catch
        {
            conn2.Close();
            return 0;//not ok
        }
    }


    public int XoaND(string taikhoan)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }

        try
        {
            string sql = "delete nguoidung_ttkd where taikhoan='" + taikhoan + "'";
            OracleCommand cmd = new OracleCommand(sql, conn2);
            cmd = new OracleCommand(sql, conn2);
            cmd.ExecuteNonQuery();
            conn2.Close();
            return 1;//da co
        }
        catch
        {
            conn2.Close();
            return 0;//chua co
        }
    }

    public int SuaND(string taikhoan, string ten, string sdt)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }

        try
        {
            string sql = "update   nguoidung_ttkd set ten='" + ten + "',sdt='" + sdt + "' where taikhoan='" + taikhoan + "'";
            OracleCommand cmd = new OracleCommand(sql, conn2);
            cmd = new OracleCommand(sql, conn2);
            cmd.ExecuteNonQuery();
            conn2.Close();
            return 1;//da co
        }
        catch
        {
            conn2.Close();
            return 0;//chua co
        }
    }




    public int ktra_suand(string taikhoan)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }

        string sql = "select * from nguoidung_ttkd where taikhoan = '" + taikhoan + "'  ";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        if (cmd.ExecuteReader().HasRows)
        {
            conn2.Close();
            return 1;//da co
        }
        else
        {
            conn2.Close();
            return 0;//chua co
        }
    }



    [WebMethod]
    public List<maker> _laytoado()
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        List<maker> tmp = new List<maker>();
        string sql = "select '1' id, 'duy' name, 'HY' address, '20.8173702295596' lat ,'106.10187123604518' lng, 'bar' type from dual UNION ALL select '2' id, 'duy2' name, 'HY' address, '20.818742613731555' lat ,'106.10511127633661' lng, 'bar' type from dual ";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        if (dt != null && dt.Rows.Count > 0)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                maker tr = new maker(dt.Rows[i]["id"].ToString(), dt.Rows[i]["name"].ToString(), dt.Rows[i]["address"].ToString(), dt.Rows[i]["lat"].ToString(), dt.Rows[i]["lng"].ToString(), dt.Rows[i]["type"].ToString());
                tmp.Add(tr);
            }
        }






        return tmp;

    }

    [WebMethod]
    public DataTable executeSqlQuery(string sql)
    {
        try
        {
            if (conn2.State == ConnectionState.Closed)
            {
                conn2.Open();
            }
            OracleCommand cmd = new OracleCommand(sql, conn2);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = new DataTable();
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            conn2.Close();
            //dt.TableName = "EmployeesList";
            return dt;
        }
        catch
        {
            conn2.Close();
            return null;
        }
    }

    [WebMethod]
    public string executeNonQuery(string strQuery)
    {
        try
        {
            DBProcess_Oracle.Exec_str(strQuery, 4);

            return "Ok";
        }
        catch
        {
            return "Not ok";
        }
    }
    [WebMethod]
    public List<thuebao> Tracuu_danhba_v2(string ma_tb)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select a.*, b.trangthai_tb from css_hyn.db_thuebao a, css_hyn.trangthai_tb b where a.trangthaitb_id =b.trangthaitb_id and a.ma_tb = '" + ma_tb + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        List<thuebao> ltb = new List<thuebao>();


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            //  public thuebao(string _ma_tb, string _ten_tb, string _diachi_tb, string _id_tb)

            thuebao tmp = new thuebao(dt.Rows[i]["ma_tb"].ToString(), dt.Rows[i]["ten_tb"].ToString(), dt.Rows[i]["diachi_tb"].ToString(), dt.Rows[i]["trangthai_tb"].ToString());

            ltb.Insert(0, tmp);
        }
        return ltb;


    }

    [WebMethod]
    public void Update_danhsachtbmll(string ngay_ks, string ngay_congtac, string nguyennhan_id, string nguyennhan, string nguoinhap,string ma_tb,string ghichu)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }

        try
        {
            string sql = "update danhsachtb_matluuluong_daks set ngay_ks = sysdate, ngay_congtac  = to_date('" + ngay_congtac + "','dd/MM/yyyy'), nguyennhan_id  = '" + nguyennhan_id + "', nguyennhan  = '" + nguyennhan + "', nguoinhap  = '" + nguoinhap + "', ghichu = '"+ghichu+"' where id in (select id from danhsachtb_matluuluong_daks where ma_tb='"+ma_tb+"' and id is not null)";
            OracleCommand cmd = new OracleCommand(sql, conn2);
            cmd = new OracleCommand(sql, conn2);
            cmd.ExecuteNonQuery();
            conn2.Close();
        }
        catch
        {
            conn2.Close();
        }
    }
	
    [WebMethod]
    public void CapNhatCongXau_TBKLuuLuong_BYNCMLL(string vnhanvien_id,string vma_tb)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }

        try
        {
            string sql = "UPDATE tinhcuoc.achatluong_gpon_chinh SET nghiemthu = '1', nhanvien_th_id =  = '"+vnhanvien_id+"', nguyennhan_id = '50', ngay_ht = sysdate WHERE ma_tb='"+vma_tb+"' and nguyennhan_id is null";
            OracleCommand cmd = new OracleCommand(sql, conn2);
            cmd = new OracleCommand(sql, conn2);
            cmd.ExecuteNonQuery();
			
			sql = "UPDATE tinhcuoc.achatluong_gpon_chinh SET nghiemthu_hyn = '1', nhanvienth_hyn =  = '"+vnhanvien_id+"',nguyennhan_id_hyn = '50',ngay_ht_hyn = sysdate WHERE ma_tb='"+vma_tb+"' and nguyennhan_id_hyn is null";
            cmd = new OracleCommand(sql, conn2);
            cmd.ExecuteNonQuery();
			
            conn2.Close();
        }
        catch
        {
            conn2.Close();
        }
    }	
	
	
	


    [WebMethod]
    public DataTable DanhSachTBMLL(string nhanvien_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        try
        {
            string sql = "select a.*,b.ghichu from ( select a.id,d.ten_kv khuvuc,f.ten_nv nv_xuly,k.tendslam,k.system, h.slot||'/'||h.port||'/'||l.vpi port, b.ma_tb,b.ten_tb,b.diachi_tb ,trim(m.sdt) sdt,trim(m.tt_hu) tt_hu,k.ip,k.loai_dslam_id from baocao_hyn.danhsachtb_matluuluong_daks a, css_hyn.db_thuebao b, css_hyn.dbtb_kv c, css_hyn.khuvuc d, admin_hyn.nhanvien f, css_hyn.db_adsl g, css_hyn.port h, css_hyn.dslam k, css_hyn.vci_vpi l ,  baocao_hyn.danhsachtb_matluuluong m where a.ma_tb = b.ma_tb and b.thuebao_id = c.thuebao_id  and g.thuebao_id = b.thuebao_id and g.port_id = h.port_id and h.dslam_id = k.dslam_id and l.vci_vpi_id = g.vci_vpi_id   and c.khuvuc_id = d.khuvuc_id  and a.nhanvien_id = f.nhanvien_id and a.id = m.id and a.nguyennhan_id is null  and a.nhanvien_id='"+nhanvien_id+"' ) a, (select ma_tb, ghichu from baocao_hyn.danhsachtb_matluuluong_daks where id in  ( select id from ( select max(id)id, ma_tb from baocao_hyn.danhsachtb_matluuluong_daks where nguyennhan_id is not null and id is not null group by ma_tb  ) )) b where a.ma_tb = b.ma_tb (+) ";
            OracleCommand cmd = new OracleCommand(sql, conn2);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            conn2.Close();
            return dt;
        }
        catch
        {
            DataTable dtnull = new DataTable();
            conn2.Close();
            return dtnull;
        }


    }





    [WebMethod]
    public DataTable DanhSachTBMLL_TimKiem(string nhanvien_id,string ma_tb)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        try
        {
            string sql = "select a.id,d.ten_kv khuvuc,f.ten_nv nv_xuly,k.tendslam,k.system, h.slot||'/'||h.port||'/'||l.vpi port, b.ma_tb,b.ten_tb,b.diachi_tb ,trim(m.sdt) sdt,trim(m.tt_hu) tt_hu,k.ip,k.loai_dslam_id,a.ghichu from baocao_hyn.danhsachtb_matluuluong_daks a, css_hyn.db_thuebao b, css_hyn.dbtb_kv c, css_hyn.khuvuc d, admin_hyn.nhanvien f, css_hyn.db_adsl g, css_hyn.port h, css_hyn.dslam k, css_hyn.vci_vpi l ,  baocao_hyn.danhsachtb_matluuluong m where a.ma_tb = b.ma_tb and b.thuebao_id = c.thuebao_id  and g.thuebao_id = b.thuebao_id and g.port_id = h.port_id and h.dslam_id = k.dslam_id and l.vci_vpi_id = g.vci_vpi_id   and c.khuvuc_id = d.khuvuc_id  and a.nhanvien_id = f.nhanvien_id and a.id = m.id and a.nguyennhan_id is null  and a.nhanvien_id='"+nhanvien_id+"' and a.ma_tb='"+ma_tb+"' ";
            OracleCommand cmd = new OracleCommand(sql, conn2);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            conn2.Close();
            return dt;
        }
        catch
        {
            DataTable dtnull = new DataTable();
            conn2.Close();
            return dtnull;
        }


    }








    [WebMethod]
    public int ktra_nguoidung(string ma_nd)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        try
        {
            string sql = "select c.donvi_cha_id from admin_hyn.nguoidung a,admin_hyn.nhanvien b, admin_hyn.donvi c  where a.nhanvien_id = b.nhanvien_id and b.donvi_id = c.donvi_id and a.ma_nd='" + ma_nd + "'";
            OracleCommand cmd = new OracleCommand(sql, conn2);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            int tong = Convert.ToInt32(dt.Rows[0][0].ToString());
            conn2.Close();
            return tong;

        }
        catch
        {
            conn2.Close();
            return 100;
        }
    }

    [WebMethod]
    public DataTable LanhDao(string ma_nd)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select c.donvi_cha_id,c.donvi_id,c.ten_dv from admin_hyn.nguoidung a,admin_hyn.nhanvien b, admin_hyn.donvi c  where a.nhanvien_id = b.nhanvien_id and b.donvi_id = c.donvi_id and a.ma_nd='" + ma_nd + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable laydanhsach_nhanvien(string chucvu_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select a.ma_mh, a.ten, a.sdt, a.chucvu, a.chucvu_id, a.chucvu_tat,a.nhanvien_id,a.ten||'-'||a.sdt ten_sdt from nhanvien_xephang a where a.chucvu_id = '" + chucvu_id + "' and a.chucvu like '%TTVTKV%'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable DanhSachTB_MLL(string donvi_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = " select a.id,c.ten_kv khuvuc,f.ten_nv ,k.tendslam,k.system,h.slot||'/'||h.port||'/'||l.vci||'/'||l.vpi port, b.ma_tb,b.ten_tb,b.diachi_tb,trim(a.sdt) sdt,trim(a.tt_hu) tt_hu  from  baocao_hyn.danhsachTB_matluuluong a,css_hyn.db_thuebao b, css_hyn.khuvuc c, css_hyn.dbtb_kv d, css_hyn.khuvuc_nv e,  admin_hyn.nhanvien f, css_hyn.db_adsl g, css_hyn.port h, css_hyn.dslam k, css_hyn.vci_vpi l  where trim(a.ma_Tb)  =b.ma_tb and b.thuebao_id = d.thuebao_id and c.khuvuc_id = e.khuvuc_id  and e.nhanvien_id = f.nhanvien_id and d.khuvuc_id = e.khuvuc_id and e.loainv_id=51  and g.thuebao_id = b.thuebao_id  and g.port_id = h.port_id and h.dslam_id = k.dslam_id and l.vci_vpi_id = g.vci_vpi_id and b.donvi_id in ("+donvi_id+")  and a.id not in (select id from baocao_hyn.danhsachtb_matluuluong_daks where id is not null) and a.id is not null   order by c.khuvuc_id	";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public void them_thuebaoMLL_giaoviec(string ma_tb, string giaoviec, string nhanvien_id,string id,string tt_hu)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "insert into danhsachtb_matluuluong_daks (ma_tb,giaoviec,nhanvien_id,ngay_ks,ngay_giaoviec,id,tt_hu) values ('" + ma_tb + "','" + giaoviec + "','" + nhanvien_id + "',sysdate,sysdate,'" + id + "','" + tt_hu + "')";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        cmd.ExecuteNonQuery();
        conn2.Close();
    }


    [WebMethod]
    public DataTable laysdt_nhanvien(string ma_mh)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select a.sdt,a.nhanvien_id from nhanvien_xephang a where a.ma_mh = '" + ma_mh + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public void sms_nvkythuat(string sdt, string noidung)
    {
        SMSNT.SMSQC nt = new SMSNT.SMSQC();
        nt.SendMultiSMS(sdt, noidung, "HYN2", "abc123", 1);
    }



    [WebMethod]
    public DataTable Laydanhsach_OB(string trungtam_id, string huyen_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select DISTINCT a.* from chitiet_khl_cxl a, css_hyn.db_thuebao b where a.trungtam_id = '" + trungtam_id + "' and a.tbtl_id not in (select tbtl_id from chitiet_khl_dxl) and a.ma_tb = b.ma_tb and b.donvi_id = '" + huyen_id + "' order by a.diaban";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable Laydanhsach_NhanvienOB(string dulieu_id, string donvi_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select nhanvien_id, ten_nv from ( select * from admin_hyn.nhanvien where nhanvien_id in ( select a.nhanvien_id from admin_hyn.nhanvien a, admin_hyn.nguoidung b where a.nhanvien_id = b.nhanvien_id  and a.donvi_id in(1948,1950,1951,1952,1953) and b.trangthai = 1 and a.donvi_dl_id = 0 ) UNION ALL select * from admin_hyn.nhanvien where nhanvien_id in ( select a.nhanvien_id from admin_hyn.nhanvien a, admin_hyn.nguoidung b where a.nhanvien_id = b.nhanvien_id  and b.trangthai = 1 and b.nhom_nd_id in (4,101) ) and ten_nv not like '%TTVT%' ) where  (donvi_id = " + dulieu_id + " or donvi_id in (select donvi_id from admin_hyn.donvi where donvi_cha_id in (" + donvi_id + ")))";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public void THEM_GVOB(string tbtl_id, string nhanviengiao_id, string nhanviennhan_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "insert into chitiet_khl_dxl  VALUES ('" + tbtl_id + "',sysdate,'','','','" + nhanviengiao_id + "','" + nhanviennhan_id + "','','')   ";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        cmd.ExecuteNonQuery();
        conn2.Close();
    }
    [WebMethod]
    public void XOA_GVOB(string tbtl_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "delete chitiet_khl_dxl  where  tbtl_id = '" + tbtl_id + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        cmd.ExecuteNonQuery();
        conn2.Close();
    }
    [WebMethod]
    public void UPDATE_GVOB(string tbtl_id, string nguyennhan, string giaiphap, string kinhdo, string vido, string nhanvien_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "update chitiet_khl_dxl set nguyennhan = '" + nguyennhan + "',giaiphap='" + giaiphap + "', ngay_ht = sysdate,kinhdo='" + kinhdo + "',vido='" + vido + "' where  tbtl_id = '" + tbtl_id + "' and nguoi_xly='" + nhanvien_id + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        cmd.ExecuteNonQuery();
        conn2.Close();
    }


    [WebMethod]
    public DataTable Laydanhsach_OB_DGV(string nhanvien_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select distinct b.* from chitiet_khl_dxl a, chitiet_khl_cxl b where a.tbtl_id = b.tbtl_id and a.nguoi_xly = '" + nhanvien_id + "' and a.tbtl_id not in (select tbtl_id from chitiet_khl_dxl where ngay_ht is not null)  and a.ngay_ht is null order by b.diaban,b.tbtl_id";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable LayDS_Nhom(string donvi_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select * from  nhom_dacnhiem where donvi_id in (" + donvi_id + ") ";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }
    [WebMethod]
    public int ThemNhomDN(string tennhom, string donvi_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        int ktra = 0;
        string sql = "select * from nhom_dacnhiem where lower(ten_nhom) = lower('" + tennhom + "') and donvi_id = '" + donvi_id + "' ";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        try
        {
            ktra = Convert.ToInt32(dt.Rows[0][0].ToString());
            return ktra;
        }
        catch
        {
            ktra = 0;
        }

        if (ktra == 0)
        {
            int id = 1;
            sql = "select max(nhom_id) nhom_id from nhom_dacnhiem ";
            cmd = new OracleCommand(sql, conn2);
            da = new OracleDataAdapter(cmd);
            ds = new DataSet();
            da.Fill(ds);
            dt = new DataTable();
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            try
            {
                id = Convert.ToInt32(dt.Rows[0][0].ToString()) + 1;
            }
            catch
            {
                id = 1;
            }

            sql = " insert into nhom_dacnhiem VALUES ('" + id + "','" + tennhom + "','" + donvi_id + "') ";
            cmd = new OracleCommand(sql, conn2);
            cmd.ExecuteNonQuery();
            conn2.Close();
        }
        return ktra;


    }
    [WebMethod]
    public DataTable LayDS_Huyen(string donvi_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select quan_id,ten_quan from  css_hyn.quan where quan_id in (" + donvi_id + ") ";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }
    [WebMethod]
    public DataTable LayDSTV_Nhom(string nhom_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "  SELECT a.nhom_nv_id, a.nhanvien_id, a.ten_nv, a.nhom_id,b.ten_nhom FROM nhom_dacnhiem_nv a, nhom_dacnhiem b where a.nhom_id = '" + nhom_id + "' and a.nhom_id = b.nhom_id";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }
    [WebMethod]
    public int ThemNhanVien_Nhom(string nhanvien_id, string ten_nv, string nhom_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        int ktra = 0;
        string sql = "select * from nhom_dacnhiem_nv where nhanvien_id = '" + nhanvien_id + "' and nhom_id = '" + nhom_id + "' ";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        try
        {
            ktra = Convert.ToInt32(dt.Rows[0][0].ToString());
            //mess.Show("Nhân viên "+ ten_nv + " đã có trong nhóm rồi");
            return ktra;
        }
        catch
        {
            ktra = 0;
        }

        if (ktra == 0)
        {
            int nhom_nv_id = 1;
            sql = "select max(nhom_nv_id) nhom_nv_id from nhom_dacnhiem_nv ";
            cmd = new OracleCommand(sql, conn2);
            da = new OracleDataAdapter(cmd);
            ds = new DataSet();
            da.Fill(ds);
            dt = new DataTable();
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            try
            {
                nhom_nv_id = Convert.ToInt32(dt.Rows[0][0].ToString()) + 1;
            }
            catch
            {
                nhom_nv_id = 1;
            }

            sql = " insert into nhom_dacnhiem_nv VALUES ('" + nhom_nv_id + "','" + nhanvien_id + "','" + ten_nv + "','" + nhom_id + "','0') ";
            cmd = new OracleCommand(sql, conn2);
            cmd.ExecuteNonQuery();
        }
        conn2.Close();
        return ktra;
    }
    [WebMethod]
    public void XoaNhom(string nhom_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "delete nhom_dacnhiem where nhom_id ='" + nhom_id + "' ";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        cmd.ExecuteNonQuery();
        conn2.Close();
    }
    [WebMethod]
    public void XoaThanhVienNhom_Grid(string nhom_nv_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "delete nhom_dacnhiem_nv where nhom_nv_id ='" + nhom_nv_id + "' ";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        cmd.ExecuteNonQuery();
        conn2.Close();
    }
    [WebMethod]
    public DataTable LAYSDT_NHANVIEN_OB(string nhanvien_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "  SELECT so_dt from admin_hyn.nhanvien where nhanvien_id in (" + nhanvien_id + ")";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }
    [WebMethod]
    public DataTable LAYDSNguoiGiaoViec(string tbtl_id, string nhanvien_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select nguoigiao from chitiet_khl_dxl where tbtl_id = '" + tbtl_id + "' and nguoi_xly = '" + nhanvien_id + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }
    [WebMethod]
    public DataTable LAYSDT_NGUOIXLY(string tbtl_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select distinct so_nt from chitiet_khl_cxl where tbtl_id = '" + tbtl_id + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable DanhSachMLL_ChuaGV(string trungtam_id, string donvi_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select distinct a.* from CHITIET_BHNL_CXL a, css_hyn.db_thuebao b where a.trungtam_id = '" + trungtam_id + "' and a.ma_tb = b.ma_tb and b.donvi_id = '" + donvi_id + "'  and a.id not in (select id from CHITIET_BHNL_DXL) order by a.ten_kv";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }
    [WebMethod]
    public void THEM_GVMLL(string id, string nhanviengiao_id, string nhanviennhan_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "insert into CHITIET_BHNL_DXL  VALUES ('" + id + "',sysdate,'','','','" + nhanviengiao_id + "','" + nhanviennhan_id + "','','')   ";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        cmd.ExecuteNonQuery();
        conn2.Close();
    }

    [WebMethod]
    public DataTable Laydanhsach_MLL_DGV(string nhanvien_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select distinct b.* from chitiet_bhnl_dxl a, chitiet_bhnl_cxl b  where a.id = b.id and a.nguoi_xly = '" + nhanvien_id + "' and b.id not in (select id from chitiet_bhnl_dxl where ngay_ht is not null)  and a.ngay_ht is null order by b.ten_kv,b.id";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public void UPDATE_GVMLL(string id, string nguyennhan, string giaiphap, string kinhdo, string vido, string nhanvien_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "update CHITIET_BHNL_DXL set nguyennhan = '" + nguyennhan + "',giaiphap='" + giaiphap + "', ngay_ht = sysdate,kinhdo='" + kinhdo + "',vido='" + vido + "' where  id = '" + id + "' and nguoi_xly='" + nhanvien_id + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        cmd.ExecuteNonQuery();
        conn2.Close();
    }

    [WebMethod]
    public DataTable LAYDSNguoiGiaoViec_MLL(string id, string nhanvien_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select nguoigiao from CHITIET_BHNL_DXL where id = '" + id + "' and nguoi_xly = '" + nhanvien_id + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable LAYSDT_NGUOIXLY_MLL(string id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "  select distinct SDT_NVXL from CHITIET_BHNL_CXL where id = '" + id + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable Lay_Ma_TB_MLL(string id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "  select distinct ma_tb from CHITIET_BHNL_CXL where id = '" + id + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable Lay_Ma_TB_OB(string tbtl_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "  select distinct ma_tb from CHITIET_KHL_CXL where tbtl_id = '" + tbtl_id + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable Laydanhsach_MLL_DGV_giamdoc(string nhanvien_id, string tungay, string denngay)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select distinct b.* from chitiet_bhnl_dxl a, chitiet_bhnl_cxl b  where a.id = b.id and a.nguoigiao = '" + nhanvien_id + "' and to_char(a.ngay_giao,'YYYYMMDD') >= '" + tungay + "' and to_char(a.ngay_giao,'YYYYMMDD') <= '" + denngay + "'  and a.ngay_ht is null order by b.ten_kv,b.id";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }
    [WebMethod]
    public DataTable Laydanhsach_KHKHL_DGV_giamdoc(string nhanvien_id, string tungay, string denngay)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select distinct b.* from CHITIET_KHL_DXL a, CHITIET_KHL_CXL b  where a.tbtl_id = b.tbtl_id and a.nguoigiao = '" + nhanvien_id + "'  and to_char(a.ngay_giao,'YYYYMMDD') >= '" + tungay + "' and to_char(a.ngay_giao,'YYYYMMDD') <= '" + denngay + "'  and a.ngay_ht is null order by b.diaban,b.tbtl_id";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }
    [WebMethod]
    public void Xoa_MLL(string id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "delete chitiet_bhnl_dxl where id='" + id + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        cmd.ExecuteNonQuery();
        conn2.Close();
    }
    [WebMethod]
    public void Xoa_CSKH(string tbtl_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "delete CHITIET_KHL_DXL where tbtl_id='" + tbtl_id + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        cmd.ExecuteNonQuery();
        conn2.Close();
    }

    [WebMethod]
    public DataTable Laydanhsach_huyen(string trungtam_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select quan_id,ten_quan from css_hyn.quan  where khuvuc='" + trungtam_id + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public void CapNhatNhomTruong(string nhom_nv_id, string nhom_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "update nhom_dacnhiem_nv set nhom_truong='0' where nhom_id='" + nhom_id + "' ";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        cmd.ExecuteNonQuery();

        sql = "update nhom_dacnhiem_nv set nhom_truong='1' where nhom_nv_id='" + nhom_nv_id + "' ";
        cmd = new OracleCommand(sql, conn2);
        cmd.ExecuteNonQuery();
        conn2.Close();
    }
    [WebMethod]
    public DataTable HienThiNhomTruong(string nhom_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select nhom_truong,nhom_nv_id from nhom_dacnhiem_nv where nhom_id= '" + nhom_id + "' and nhom_truong= 1 ";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable LayDsDaHoanThanh(string tungay, string denngay, string kieu, string ten_tt)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select ten_quan,ten_kv,ma_tb,ten_tb,diachi_tb,nhanvien_xl,loai_loi, ngay_giao,ngay_ht,nguyennhan, giaiphap,nvcskh, toado_cskh,decode(toado_tb,',','0,0',toado_tb)toado_tb ,kieu, ngaybaoloi, vido_cskh,kinhdo_cskh,decode(vido_tb,null,0,vido_tb)vido_tb,decode(kinhdo_tb,null,0,kinhdo_tb)kinhdo_tb from ( select DISTINCT a.ten_quan,a.ten_kv,a.ma_tb,a.ten_tb,a.diachi_tb,a.nhanvien_xl,a.loai_loi,a.ngay_giao,a.ngay_ht,a.nguyennhan, a.giaiphap,a.nvcskh, a.toado toado_cskh,a.vido vido_cskh,a.kinhdo kinhdo_cskh ,c.vido||','||c.kinhdo toado_tb,c.vido vido_tb,c.kinhdo kinhdo_tb,a.kieu, a.ngaybaoloi  from ( select a.ten_quan,a.ten_kv,a.ma_tb,a.ten_tb,a.diachi_tb,a.nhanvien_xl,a.ct_hong loai_loi, b.ngay_giao,b.ngay_ht,b.nguyennhan, b.giaiphap,c.ten_nv nvcskh, b.vido||','||b.kinhdo toado,'KH báo hỏng nhiều lần' kieu, a.ngay_bh ngaybaoloi,b.vido,  b.kinhdo from chitiet_bhnl_cxl a, chitiet_bhnl_dxl b, admin_hyn.nhanvien c where a.id = b.id and b.nguoi_xly = c.nhanvien_id and b.ngay_ht is not null  UNION ALL select a.huyen ten_quan,a.diaban ten_kv,a.ma_tb,a.ten_tb,a.diachi_tb,a.nhanvien_xl,a.loai_loi,b.ngay_giao,b.ngay_ht,b.nguyennhan,  b.giaiphap,c.ten_nv nvcskh, b.vido||','||b.kinhdo toado,'KH chưa hài lòng' kieu, a.ngay_thuchien ngaybaoloi ,b.vido,  b.kinhdo  from chitiet_khl_cxl a, chitiet_khl_dxl b, admin_hyn.nhanvien c where a.tbtl_id = b.tbtl_id  and b.nguoi_xly = c.nhanvien_id  and b.ngay_ht is not null ) a, css_hyn.db_thuebao b, css_hyn.diachi_tb c where a.ma_tb = b.ma_tb and b.thuebao_id = c.thuebao_id ) ";
        if (kieu == "0")
        {
            sql += "  where kieu in ('KH chưa hài lòng','KH báo hỏng nhiều lần')";
        }
        if (kieu == "1")
        {
            sql += "  where kieu = 'KH chưa hài lòng'";
        }
        if (kieu == "2")
        {
            sql += "  where kieu = 'KH báo hỏng nhiều lần'";
        }
        if (tungay.Trim() != "" && denngay.Trim() != "")
        {
            sql += "  and to_char(ngay_giao,'YYYYMMDD') >= '" + tungay + "'";
            sql += "  and to_char(ngay_giao,'YYYYMMDD') <= '" + denngay + "'";
        }
        if (ten_tt == "1")
        {
            sql += " and ten_kv like '%KV I.%'";
        }
        if (ten_tt == "2")
        {
            sql += " and ten_kv like '%KV II.%'";
        }
        if (ten_tt == "3")
        {
            sql += " and ten_kv like '%KV III.%'";
        }
        if (ten_tt == "4")
        {
            sql += " and ten_kv like '%KV IV.%'";
        }
        if (ten_tt == "5")
        {
            sql += " and ten_kv like '%KV V.%'";
        }
        sql += "  order by ten_kv,ma_tb";

        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }
    [WebMethod]
    public DataTable LayDsDaGV(string tungay, string denngay, string kieu, string ten_tt)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = " select distinct * from ( select * from ( select ten_quan,ten_kv,ma_tb,ten_tb,diachi_tb,sdt_kh,nhanvien_xl,loai_loi,ngay_giao,ngay_ht,nguyennhan,  giaiphap,nvcskh,toado,kieu,ngaybaoloi   from (  select a.ten_quan,a.ten_kv,a.ma_tb,a.ten_tb,a.diachi_tb,a.nhanvien_xl,a.ct_hong loai_loi,b.ngay_giao,b.ngay_ht,b.nguyennhan,a.sdt_kh,  b.giaiphap,c.ten_nv nvcskh, '' toado,'KH báo hỏng nhiều lần' kieu, a.ngay_bh ngaybaoloi   from chitiet_bhnl_cxl a, chitiet_bhnl_dxl b, admin_hyn.nhanvien c, nhom_dacnhiem_nv d   where a.id = b.id and b.nguoi_xly = c.nhanvien_id and c.nhanvien_id = d.nhanvien_id and d.nhom_truong=1 and b.ngay_ht is null  and a.id not in (select id from chitiet_bhnl_dxl where ngay_ht is not null)  and a.id in (select DISTINCT id from chitiet_bhnl_dxl group by id having count(*)> 1) UNION ALL   select a.huyen ten_quan,a.diaban ten_kv,a.ma_tb,a.ten_tb,a.diachi_tb,a.nhanvien_xl,a.loai_loi,b.ngay_giao,b.ngay_ht,b.nguyennhan,a.dienthoai_lh sdt_kh, b.giaiphap,c.ten_nv nvcskh,  '' toado,'KH chưa hài lòng' kieu, a.ngay_thuchien ngaybaoloi     from chitiet_khl_cxl a, chitiet_khl_dxl b, admin_hyn.nhanvien c , nhom_dacnhiem_nv d   where a.tbtl_id = b.tbtl_id and b.nguoi_xly = c.nhanvien_id and c.nhanvien_id = d.nhanvien_id and d.nhom_truong=1 and b.ngay_ht is null  and a.tbtl_id not in (select tbtl_id from chitiet_khl_dxl where ngay_ht is not null) and a.tbtl_id in (select DISTINCT tbtl_id from chitiet_khl_dxl group by tbtl_id having count(*)> 1) ) ) UNION ALL select * from ( select ten_quan,ten_kv,ma_tb,ten_tb,diachi_tb,sdt_kh,nhanvien_xl,loai_loi,ngay_giao,ngay_ht,nguyennhan, giaiphap,nvcskh,toado,kieu,ngaybaoloi   from ( select a.ten_quan,a.ten_kv,a.ma_tb,a.ten_tb,a.diachi_tb,a.nhanvien_xl,a.ct_hong loai_loi,b.ngay_giao,b.ngay_ht,b.nguyennhan,a.sdt_kh, b.giaiphap,c.ten_nv nvcskh, '' toado,'KH báo hỏng nhiều lần' kieu, a.ngay_bh ngaybaoloi  from chitiet_bhnl_cxl a, chitiet_bhnl_dxl b, admin_hyn.nhanvien c where a.id = b.id and b.nguoi_xly = c.nhanvien_id and b.ngay_ht is null and a.id not in (select id from chitiet_bhnl_dxl where ngay_ht is not null) and a.id not in (select DISTINCT id from chitiet_bhnl_dxl group by id having count(*)> 1)  UNION ALL  select a.huyen ten_quan,a.diaban ten_kv,a.ma_tb,a.ten_tb,a.diachi_tb,a.nhanvien_xl,a.loai_loi,b.ngay_giao,b.ngay_ht,b.nguyennhan, a.dienthoai_lh sdt_kh, b.giaiphap,c.ten_nv nvcskh, '' toado,'KH chưa hài lòng' kieu, a.ngay_thuchien ngaybaoloi    from chitiet_khl_cxl a, chitiet_khl_dxl b, admin_hyn.nhanvien c  where a.tbtl_id = b.tbtl_id and b.nguoi_xly = c.nhanvien_id  and b.ngay_ht is null  and a.tbtl_id not in (select tbtl_id from chitiet_khl_dxl where ngay_ht is not null) and a.tbtl_id not in (select DISTINCT tbtl_id from chitiet_khl_dxl group by tbtl_id having count(*)> 1) ) ) )";
        if (kieu == "0")
        {
            sql += "  where kieu in ('KH chưa hài lòng','KH báo hỏng nhiều lần')";
        }
        if (kieu == "1")
        {
            sql += "  where kieu = 'KH chưa hài lòng'";
        }
        if (kieu == "2")
        {
            sql += "  where kieu = 'KH báo hỏng nhiều lần'";
        }
        if (tungay.Trim() != "" && denngay.Trim() != "")
        {
            sql += "  and to_char(ngay_giao,'YYYYMMDD') >= '" + tungay + "'";
            sql += "  and to_char(ngay_giao,'YYYYMMDD') <= '" + denngay + "'";
        }
        if (ten_tt == "1")
        {
            sql += " and ten_kv like '%KV I.%'";
        }
        if (ten_tt == "2")
        {
            sql += " and ten_kv like '%KV II.%'";
        }
        if (ten_tt == "3")
        {
            sql += " and ten_kv like '%KV III.%'";
        }
        if (ten_tt == "4")
        {
            sql += " and ten_kv like '%KV IV.%'";
        }
        if (ten_tt == "5")
        {
            sql += " and ten_kv like '%KV V.%'";
        }
        sql += "  order by ten_kv,ma_tb";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable LayDsChuaGV(string kieu, string ten_tt)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select distinct * from ( select a.ten_quan,a.ten_kv,a.ma_tb,a.ten_tb,a.diachi_tb,a.nhanvien_xl,a.ct_hong loai_loi,'' ngay_giao,'' ngay_ht,'' nguyennhan, '' giaiphap,'' nvcskh, '' toado,'KH báo h?ng nhi?u l?n' kieu, ngay_bh ngaybaoloi,sdt_kh    from CHITIET_BHNL_CXL a where a.id not in (select id from chitiet_bhnl_dxl)  UNION ALL  select a.huyen ten_quan,a.diaban ten_kv,a.ma_tb,a.ten_tb,a.diachi_tb,a.nhanvien_xl,a.loai_loi,'' ngay_giao,'' ngay_ht,'' nguyennhan, '' giaiphap,'' nvcskh, '' toado,'KH chua hài lòng' kieu ,ngay_thuchien ngaybaoloi, dienthoai_lh sdt_kh   from chitiet_khl_cxl a where a.tbtl_id not in (select tbtl_id from chitiet_khl_dxl) ) ";
        if (kieu == "0")
        {
            sql += "  where kieu in ('KH chưa hài lòng','KH báo hỏng nhiều lần')";
        }
        if (kieu == "1")
        {
            sql += "  where kieu = 'KH chưa hài lòng'";
        }
        if (kieu == "2")
        {
            sql += "  where kieu = 'KH báo hỏng nhiều lần'";
        }
        if (ten_tt == "1")
        {
            sql += " and ten_kv like '%KV I.%'";
        }
        if (ten_tt == "2")
        {
            sql += " and ten_kv like '%KV II.%'";
        }
        if (ten_tt == "3")
        {
            sql += " and ten_kv like '%KV III.%'";
        }
        if (ten_tt == "4")
        {
            sql += " and ten_kv like '%KV IV.%'";
        }
        if (ten_tt == "5")
        {
            sql += " and ten_kv like '%KV V.%'";
        }
        sql += "  order by ten_kv, ma_tb";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable BaoCaoTheoDichVu(string thang)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        //string sql = "select trungtam_id,ten_trungtam,tong_khl,tong_bhnl, tong_khl_dxl,tong_bhnl_dxl,tong_khl_dgv,tong_bhnl_dgv, (tong_khl-tong_khl_dxl-tong_khl_dgv)tong_khl_cgv, (tong_bhnl-tong_bhnl_dxl-tong_bhnl_dgv)tong_bhnl_cgv from ( select b.trungtam_id,b.ten_trungtam,a.tong_khl,a.tong_bhnl, a.tong_khl_dxl,a.tong_bhnl_dxl,a.tong_khl_dgv,a.tong_bhnl_dgv from ( select a.trungtam_id,a.tong_khl,a.tong_bhnl, b.tong_khl_dxl,b.tong_bhnl_dxl,c.tong_khl_dgv,c.tong_bhnl_dgv from ( select a.trungtam_id,decode(tong_khl,null,0,tong_khl)tong_khl,decode(tong_bhnl,null,0,tong_bhnl)tong_bhnl from (select trungtam_id,sum(tong_khl)tong_khl, sum(tong_bhnl)tong_bhnl from ( select trungtam_id,count(*)tong_khl, 0 tong_bhnl from ( select distinct ma_tb,trungtam_id from chitiet_khl_cxl where to_char(ngay_thuchien,'YYYYMM')='"+thang+"' ) group by trungtam_id UNION ALL  select trungtam_id,0 tong_khl,count(*) tong_bhnl from ( select distinct ma_tb,trungtam_id from chitiet_bhnl_cxl where to_char(ngay_bh,'YYYYMM')='"+thang+"' ) group by trungtam_id ) group by trungtam_id ) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id (+) ) a, ( select b.trungtam_id,decode(tong_khl_dxl,null,0,tong_khl_dxl)tong_khl_dxl,decode(tong_bhnl_dxl,null,0,tong_bhnl_dxl)tong_bhnl_dxl from (select trungtam_id,sum(tong_khl_dxl)tong_khl_dxl,sum(tong_bhnl_dxl)tong_bhnl_dxl from ( select trungtam_id,count(*)tong_khl_dxl, 0 tong_bhnl_dxl from ( select distinct a.ma_tb,a.trungtam_id from chitiet_khl_cxl a, chitiet_khl_dxl b where a.tbtl_id = b.tbtl_id and b.ngay_ht is not null  and to_char(a.ngay_thuchien,'YYYYMM')='"+thang+"' ) group by trungtam_id UNION ALL select trungtam_id,0 tong_khl_dxl,count(*) tong_bhnl_dxl from ( select distinct a.ma_tb,a.trungtam_id from chitiet_bhnl_cxl a, chitiet_bhnl_dxl b where a.id = b.id and b.ngay_ht is not null and to_char(a.ngay_bh,'YYYYMM')='"+thang+"' ) group by trungtam_id ) group by trungtam_id ) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id (+) ) b, ( select  b.trungtam_id,decode(tong_khl_dgv,null,0,tong_khl_dgv)tong_khl_dgv,decode(tong_bhnl_dgv,null,0,tong_bhnl_dgv)tong_bhnl_dgv from (select trungtam_id,sum(tong_khl_dgv)tong_khl_dgv,sum(tong_bhnl_dgv)tong_bhnl_dgv from ( select trungtam_id,count(*)tong_khl_dgv, 0 tong_bhnl_dgv from ( select distinct a.ma_tb,a.trungtam_id from chitiet_khl_cxl a, chitiet_khl_dxl b where a.tbtl_id = b.tbtl_id and b.ngay_ht is null  and a.tbtl_id not in (select tbtl_id from chitiet_khl_dxl where ngay_ht is not null and to_char(ngay_giao,'YYYYMM')='"+thang+"') and to_char(a.ngay_thuchien,'YYYYMM')='"+thang+"' ) group by trungtam_id UNION ALL select trungtam_id,0 tong_khl_dgv,count(*) tong_bhnl_dgv from ( select distinct a.ma_tb,a.trungtam_id from chitiet_bhnl_cxl a, chitiet_bhnl_dxl b where a.id = b.id and b.ngay_ht is null and a.id not in (select id from chitiet_bhnl_dxl where ngay_ht is not null and to_char(ngay_giao,'YYYYMM')='"+thang+"') and to_char(a.ngay_bh,'YYYYMM')='"+thang+"' ) group by trungtam_id )  group by trungtam_id) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id (+) ) c where a.trungtam_id = b.trungtam_id and  a.trungtam_id = c.trungtam_id  ) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id  ) order by trungtam_id ";
        string sql = "select b.trungtam_id,b.ten_trungtam,a.tong_khl,a.tong_bhnl, a.tong_khl_dxl,a.tong_bhnl_dxl,a.tong_khl_dgv,a.tong_bhnl_dgv,a.tong_khl_cgv,a.tong_bhnl_cgv from (  select a.trungtam_id,a.tong_khl,a.tong_bhnl, b.tong_khl_dxl,b.tong_bhnl_dxl,c.tong_khl_dgv,c.tong_bhnl_dgv,d.tong_khl_cgv,d.tong_bhnl_cgv from ( select a.trungtam_id,decode(tong_khl,null,0,tong_khl)tong_khl,decode(tong_bhnl,null,0,tong_bhnl)tong_bhnl from (select trungtam_id,sum(tong_khl)tong_khl, sum(tong_bhnl)tong_bhnl from ( select trungtam_id,count(*)tong_khl, 0 tong_bhnl from ( select distinct ma_tb,trungtam_id from chitiet_khl_cxl where to_char(ngaylaydl,'YYYYMM')='" + thang + "' ) group by trungtam_id UNION ALL  select trungtam_id,0 tong_khl,count(*) tong_bhnl from ( select distinct ma_tb,trungtam_id from chitiet_bhnl_cxl where to_char(ngaylaydl,'YYYYMM')='" + thang + "' )  group by trungtam_id ) group by trungtam_id ) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id (+) ) a,  ( select b.trungtam_id,decode(tong_khl_dxl,null,0,tong_khl_dxl)tong_khl_dxl,decode(tong_bhnl_dxl,null,0,tong_bhnl_dxl)tong_bhnl_dxl from (select trungtam_id,sum(tong_khl_dxl)tong_khl_dxl,sum(tong_bhnl_dxl)tong_bhnl_dxl from ( select trungtam_id,count(*)tong_khl_dxl, 0 tong_bhnl_dxl from ( select distinct a.ma_tb,a.trungtam_id from chitiet_khl_cxl a, chitiet_khl_dxl b where a.tbtl_id = b.tbtl_id and b.ngay_ht is not null  and to_char(b.ngay_giao,'YYYYMM')='" + thang + "' ) group by trungtam_id UNION ALL select trungtam_id,0 tong_khl_dxl,count(*) tong_bhnl_dxl from ( select distinct a.ma_tb,a.trungtam_id from chitiet_bhnl_cxl a, chitiet_bhnl_dxl b where a.id = b.id and b.ngay_ht is not null and to_char(b.ngay_giao,'YYYYMM')='" + thang + "' ) group by trungtam_id ) group by trungtam_id ) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id (+) ) b, ( select  b.trungtam_id,decode(tong_khl_dgv,null,0,tong_khl_dgv)tong_khl_dgv,decode(tong_bhnl_dgv,null,0,tong_bhnl_dgv)tong_bhnl_dgv from ( select trungtam_id,sum(tong_khl_dgv)tong_khl_dgv,sum(tong_bhnl_dgv)tong_bhnl_dgv from ( select trungtam_id,count(*)tong_khl_dgv, 0 tong_bhnl_dgv from ( select distinct a.ma_tb,a.trungtam_id from chitiet_khl_cxl a, chitiet_khl_dxl b where a.tbtl_id = b.tbtl_id and b.ngay_ht is null  and a.tbtl_id not in (select tbtl_id from chitiet_khl_dxl where ngay_ht is not null) and to_char(b.ngay_giao,'YYYYMM')='" + thang + "' ) group by trungtam_id UNION ALL select trungtam_id,0 tong_khl_dgv,count(*) tong_bhnl_dgv from ( select distinct a.ma_tb,a.trungtam_id from chitiet_bhnl_cxl a, chitiet_bhnl_dxl b where a.id = b.id and b.ngay_ht is null and a.id not in (select id from chitiet_bhnl_dxl where ngay_ht is not null) and to_char(b.ngay_giao,'YYYYMM')='" + thang + "' ) group by trungtam_id )  group by trungtam_id) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id (+) ) c , ( select trungtam_id,tong_khl_cgv,tong_bhnl_cgv from ( select trungtam_id,sum(tong_khl_cgv)tong_khl_cgv,sum(tong_bhnl_cgv)tong_bhnl_cgv from ( ( select  b.trungtam_id,decode(tong_khl_cgv,null,0,tong_khl_cgv)tong_khl_cgv,decode(tong_bhnl_cgv,null,0,tong_bhnl_cgv)tong_bhnl_cgv from (  select trungtam_id,count(*)tong_khl_cgv, 0 tong_bhnl_cgv from (  select  distinct ma_tb,trungtam_id from chitiet_khl_cxl  where tbtl_id not in ( select tbtl_id from chitiet_khl_dxl where to_char(ngay_giao,'YYYYMM')='" + thang + "') and to_char(ngaylaydl,'YYYYMM')='" + thang + "' )  group by trungtam_id  UNION ALL  select trungtam_id,0 tong_khl_cgv, count(*) tong_bhnl_cgv from (  select  distinct ma_tb,trungtam_id from chitiet_bhnl_cxl  where id not in (select id from chitiet_bhnl_dxl where to_char(ngay_giao,'YYYYMM')='" + thang + "') and to_char(ngaylaydl,'YYYYMM')='" + thang + "' ) group by trungtam_id ) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id (+) )  ) group by trungtam_id ) ) d where a.trungtam_id = b.trungtam_id and  a.trungtam_id = c.trungtam_id and a.trungtam_id =d.trungtam_id  ) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }
    [WebMethod]
    public DataTable BaoCaoTheoKhachHang(string thang)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        //string sql = "select trungtam_id,ten_trungtam,tong_khl,tong_bhnl, tong_khl_dxl,tong_bhnl_dxl,tong_khl_dgv,tong_bhnl_dgv, (tong_khl-tong_khl_dxl-tong_khl_dgv)tong_khl_cgv, (tong_bhnl-tong_bhnl_dxl-tong_bhnl_dgv)tong_bhnl_cgv from ( select b.trungtam_id,b.ten_trungtam,a.tong_khl,a.tong_bhnl, a.tong_khl_dxl,a.tong_bhnl_dxl,a.tong_khl_dgv,a.tong_bhnl_dgv from ( select a.trungtam_id,a.tong_khl,a.tong_bhnl, b.tong_khl_dxl,b.tong_bhnl_dxl,c.tong_khl_dgv,c.tong_bhnl_dgv from ( select b.trungtam_id,decode(tong_khl,null,0,tong_khl)tong_khl,decode(tong_bhnl,null,0,tong_bhnl)tong_bhnl from (select trungtam_id,sum(tong_khl)tong_khl, sum(tong_bhnl)tong_bhnl from ( select trungtam_id,count(*)tong_khl, 0 tong_bhnl from ( select distinct tbtl_id,trungtam_id from chitiet_khl_cxl where to_char(ngay_thuchien,'YYYYMM')='"+thang+"' ) group by trungtam_id  UNION ALL  select trungtam_id,0 tong_khl,count(*) tong_bhnl from ( select distinct ma_tb,trungtam_id from chitiet_bhnl_cxl where to_char(ngay_bh,'YYYYMM')='"+thang+"' ) group by trungtam_id ) group by trungtam_id) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id (+)) a, ( select b.trungtam_id,decode(tong_khl_dxl,null,0,tong_khl_dxl)tong_khl_dxl,decode(tong_bhnl_dxl,null,0,tong_bhnl_dxl)tong_bhnl_dxl from (select trungtam_id,sum(tong_khl_dxl)tong_khl_dxl,sum(tong_bhnl_dxl)tong_bhnl_dxl from ( select trungtam_id,count(*)tong_khl_dxl, 0 tong_bhnl_dxl from ( select distinct a.tbtl_id,a.trungtam_id from chitiet_khl_cxl a, chitiet_khl_dxl b where a.tbtl_id = b.tbtl_id and b.ngay_ht is not null  and to_char(a.ngay_thuchien,'YYYYMM')='"+thang+"' ) group by trungtam_id UNION ALL select trungtam_id,0 tong_khl_dxl,count(*) tong_bhnl_dxl from ( select distinct a.ma_tb,a.trungtam_id from chitiet_bhnl_cxl a, chitiet_bhnl_dxl b where a.id = b.id and b.ngay_ht is not null and to_char(a.ngay_bh,'YYYYMM')='"+thang+"' ) group by trungtam_id ) group by trungtam_id ) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id (+)) b, ( select b.trungtam_id,decode(tong_khl_dgv,null,0,tong_khl_dgv)tong_khl_dgv,decode(tong_bhnl_dgv,null,0,tong_bhnl_dgv)tong_bhnl_dgv from ( select trungtam_id,sum(tong_khl_dgv)tong_khl_dgv,sum(tong_bhnl_dgv)tong_bhnl_dgv from ( select trungtam_id,count(*)tong_khl_dgv, 0 tong_bhnl_dgv from ( select distinct a.tbtl_id,a.trungtam_id from chitiet_khl_cxl a, chitiet_khl_dxl b where a.tbtl_id = b.tbtl_id and b.ngay_ht is null  and a.tbtl_id not in (select tbtl_id from chitiet_khl_dxl where ngay_ht is not null and to_char(ngay_giao,'YYYYMM')='"+thang+"') and to_char(a.ngay_thuchien,'YYYYMM')='"+thang+"' ) group by trungtam_id UNION ALL select trungtam_id,0 tong_khl_dgv,count(*) tong_bhnl_dgv from ( select distinct a.ma_tb,a.trungtam_id from chitiet_bhnl_cxl a, chitiet_bhnl_dxl b where a.id = b.id and b.ngay_ht is null and a.id not in (select id from chitiet_bhnl_dxl where ngay_ht is not null and to_char(ngay_giao,'YYYYMM')='"+thang+"') and to_char(a.ngay_bh,'YYYYMM')='"+thang+"' ) group by trungtam_id ) group by trungtam_id ) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id (+)) c where a.trungtam_id = b.trungtam_id and  a.trungtam_id = c.trungtam_id ) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id (+) ) order by trungtam_id";
        string sql = "select b.trungtam_id,b.ten_trungtam,a.tong_khl,a.tong_bhnl, a.tong_khl_dxl,a.tong_bhnl_dxl,a.tong_khl_dgv,a.tong_bhnl_dgv,a.tong_khl_cgv,a.tong_bhnl_cgv from ( select a.trungtam_id,a.tong_khl,a.tong_bhnl, b.tong_khl_dxl,b.tong_bhnl_dxl,c.tong_khl_dgv,c.tong_bhnl_dgv,d.tong_khl_cgv,d.tong_bhnl_cgv from ( select a.trungtam_id,decode(tong_khl,null,0,tong_khl)tong_khl,decode(tong_bhnl,null,0,tong_bhnl)tong_bhnl from (select trungtam_id,sum(tong_khl)tong_khl, sum(tong_bhnl)tong_bhnl from ( select trungtam_id,count(*)tong_khl, 0 tong_bhnl from ( select distinct tbtl_id,trungtam_id from chitiet_khl_cxl where to_char(ngaylaydl,'YYYYMM')='" + thang + "' ) group by trungtam_id UNION ALL  select trungtam_id,0 tong_khl,count(*) tong_bhnl from ( select distinct id,trungtam_id from chitiet_bhnl_cxl where to_char(ngaylaydl,'YYYYMM')='" + thang + "' ) group by trungtam_id ) group by trungtam_id ) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id (+) ) a, ( select b.trungtam_id,decode(tong_khl_dxl,null,0,tong_khl_dxl)tong_khl_dxl,decode(tong_bhnl_dxl,null,0,tong_bhnl_dxl)tong_bhnl_dxl from (select trungtam_id,sum(tong_khl_dxl)tong_khl_dxl,sum(tong_bhnl_dxl)tong_bhnl_dxl from ( select trungtam_id,count(*)tong_khl_dxl, 0 tong_bhnl_dxl from ( select distinct a.tbtl_id,a.trungtam_id from chitiet_khl_cxl a, chitiet_khl_dxl b where a.tbtl_id = b.tbtl_id and b.ngay_ht is not null  and to_char(b.ngay_giao,'YYYYMM')='" + thang + "' ) group by trungtam_id UNION ALL select trungtam_id,0 tong_khl_dxl,count(*) tong_bhnl_dxl from ( select distinct a.id,a.trungtam_id from chitiet_bhnl_cxl a, chitiet_bhnl_dxl b where a.id = b.id and b.ngay_ht is not null and to_char(b.ngay_giao,'YYYYMM')='" + thang + "' ) group by trungtam_id ) group by trungtam_id ) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id (+) ) b, (  select  b.trungtam_id,decode(tong_khl_dgv,null,0,tong_khl_dgv)tong_khl_dgv,decode(tong_bhnl_dgv,null,0,tong_bhnl_dgv)tong_bhnl_dgv from ( select trungtam_id,sum(tong_khl_dgv)tong_khl_dgv,sum(tong_bhnl_dgv)tong_bhnl_dgv from (  select trungtam_id,count(*)tong_khl_dgv, 0 tong_bhnl_dgv from (  select distinct a.tbtl_id,a.trungtam_id from chitiet_khl_cxl a, chitiet_khl_dxl b  where a.tbtl_id = b.tbtl_id and b.ngay_ht is null   and a.tbtl_id not in (select tbtl_id from chitiet_khl_dxl where ngay_ht is not null)  and to_char(b.ngay_giao,'YYYYMM')='" + thang + "' ) group by trungtam_id  UNION ALL  select trungtam_id,0 tong_khl_dgv,count(*) tong_bhnl_dgv from (  select distinct a.id,a.trungtam_id from chitiet_bhnl_cxl a, chitiet_bhnl_dxl b  where a.id = b.id and b.ngay_ht is null and a.id not in (select id from chitiet_bhnl_dxl where ngay_ht is not null)  and to_char(b.ngay_giao,'YYYYMM')='" + thang + "' ) group by trungtam_id )   group by trungtam_id) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id (+) ) c , ( select  b.trungtam_id,decode(tong_khl_cgv,null,0,tong_khl_cgv)tong_khl_cgv,decode(tong_bhnl_cgv,null,0,tong_bhnl_cgv)tong_bhnl_cgv from ( select trungtam_id,count(*)tong_khl_cgv, 0 tong_bhnl_cgv from (  select  distinct tbtl_id,trungtam_id from chitiet_khl_cxl  where tbtl_id not in (select tbtl_id from chitiet_khl_dxl where to_char(ngay_giao,'YYYYMM')='" + thang + "') and to_char(ngaylaydl,'YYYYMM')='" + thang + "' ) group by trungtam_id UNION ALL select trungtam_id,0 tong_khl_cgv, count(*) tong_bhnl_cgv from (  select  distinct id,trungtam_id from chitiet_bhnl_cxl  where id not in (select id from chitiet_bhnl_dxl where to_char(ngay_giao,'YYYYMM')='" + thang + "') and to_char(ngaylaydl,'YYYYMM')='" + thang + "' ) group by trungtam_id ) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id (+) ) d where a.trungtam_id = b.trungtam_id and  a.trungtam_id = c.trungtam_id and a.trungtam_id =d.trungtam_id  ) a, tinhcuoc.qt_trungtam b where b.trungtam_id = a.trungtam_id";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable ThoiGianCSKH(string tungay, string denngay)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select a.trungtam_id,a.ten_trungtam,Round(a.min_phut_bh/60,2)min_gio_bh,Round(a.max_phut_bh/60,2)max_gio_bh, Round(a.tg_tbbh/60,2)tg_tbbh,Round(b.min_phut_cskh/60,2)min_gio_cskh,Round(b.max_phut_cskh/60,2)max_gio_cskh, Round(b.tg_tb_cskh/60,2)tg_tb_cskh from ( select a.trungtam_id,a.ten_trungtam,b.min_phut_bh,b.max_phut_bh,b.tong_phut,b.tong_tb,Round((b.tong_phut/b.tong_tb),2) tg_tbbh from tinhcuoc.qt_trungtam a, (select a.*,b.tong_phut,b.tong_tb from ( select trungtam_id,min(phut)min_phut_bh, max(phut)max_phut_bh from ( select trungtam_id,ngay_ht,ngay_bh, (ngay*24*60 + hrs*60+mins)phut from ( select b.trungtam_id,a.ngay_ht,b.ngay_bh, trunc(a.ngay_ht - add_months( b.ngay_bh, months_between(a.ngay_ht,b.ngay_bh))) as ngay, trunc(24*mod(a.ngay_ht - b.ngay_bh,1)) as hrs, trunc( mod(mod(a.ngay_ht - b.ngay_bh,1)*24,1)*60 ) as mins  from chitiet_bhnl_dxl a, chitiet_bhnl_cxl b where a.id = b.id and a.ngay_ht is not null  and to_char(a.ngay_ht,'YYYYMMDD')>='" + tungay + "' and to_char(a.ngay_ht,'YYYYMMDD')<='" + denngay + "' ) ) group by trungtam_id ) a,  ( select trungtam_id,sum(phut) tong_phut,count(*)tong_tb from ( select trungtam_id,ngay_ht,ngay_bh, (ngay*24*60 + hrs*60+mins)phut from ( select b.trungtam_id,a.ngay_ht,b.ngay_bh, trunc(a.ngay_ht - add_months( b.ngay_bh, months_between(a.ngay_ht,b.ngay_bh))) as ngay, trunc(24*mod(a.ngay_ht - b.ngay_bh,1)) as hrs, trunc( mod(mod(a.ngay_ht - b.ngay_bh,1)*24,1)*60 ) as mins  from chitiet_bhnl_dxl a, chitiet_bhnl_cxl b where a.id = b.id and a.ngay_ht is not null and to_char(a.ngay_ht,'YYYYMMDD')>='" + tungay + "' and to_char(a.ngay_ht,'YYYYMMDD')<='" + denngay + "' ) ) group by trungtam_id  ) b where a.trungtam_id = b.trungtam_id ) b where a.trungtam_id = b.trungtam_id (+) order by a.trungtam_id ) a, ( select a.trungtam_id,a.ten_trungtam,b.min_phut_cskh,b.max_phut_cskh,b.tong_phut,b.tong_tb,Round((b.tong_phut/b.tong_tb),2) tg_tb_cskh from tinhcuoc.qt_trungtam a, ( select a.*,b.tong_phut,b.tong_tb from ( select trungtam_id,min(phut)min_phut_cskh, max(phut)max_phut_cskh from ( select trungtam_id,ngay_ht,ngay_thuchien, (ngay*24*60 + hrs*60+mins)phut from ( select b.trungtam_id,a.ngay_ht,b.ngay_thuchien, trunc(a.ngay_ht - add_months( b.ngay_thuchien, months_between(a.ngay_ht,b.ngay_thuchien))) as ngay, trunc(24*mod(a.ngay_ht - b.ngay_thuchien,1)) as hrs, trunc( mod(mod(a.ngay_ht - b.ngay_thuchien,1)*24,1)*60 ) as mins  from chitiet_khl_dxl a, chitiet_khl_cxl b where a.tbtl_id = b.tbtl_id and a.ngay_ht is not null and to_char(a.ngay_ht,'YYYYMMDD')>='" + tungay + "' and to_char(a.ngay_ht,'YYYYMMDD')<='" + denngay + "' ) ) group by trungtam_id ) a,  ( select trungtam_id,sum(phut) tong_phut,count(*)tong_tb from ( select trungtam_id,ngay_ht,ngay_thuchien, (ngay*24*60 + hrs*60+mins)phut from ( select b.trungtam_id,a.ngay_ht,b.ngay_thuchien, trunc(a.ngay_ht - add_months( b.ngay_thuchien, months_between(a.ngay_ht,b.ngay_thuchien))) as ngay, trunc(24*mod(a.ngay_ht - b.ngay_thuchien,1)) as hrs, trunc( mod(mod(a.ngay_ht - b.ngay_thuchien,1)*24,1)*60 ) as mins  from chitiet_khl_dxl a, chitiet_khl_cxl b where a.tbtl_id = b.tbtl_id and a.ngay_ht is not null and to_char(a.ngay_ht,'YYYYMMDD')>='" + tungay + "' and to_char(a.ngay_ht,'YYYYMMDD')<='" + denngay + "' ) ) group by trungtam_id  ) b where a.trungtam_id = b.trungtam_id ) b where a.trungtam_id = b.trungtam_id (+) order by a.trungtam_id ) b where a.trungtam_id = b.trungtam_id ";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable LayNhanVienToDacNhiem(string nhom_id)
    {

        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select * from (select 0 nhanvien_id,'Tất cả nhân viên' ten_nv from dual UNION ALL select nhanvien_id,ten_nv from nhom_dacnhiem_nv where nhom_id = '" + nhom_id + "' ) order by nhanvien_id ";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }



    #region dashboard
    [WebMethod]
    public DataTable Lay_ds_phattrienmoi_tonghop(string vtungay, string vdenngay, string vloaitb_id, string vdonvi_id,string vkieu)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.Lay_ds_phattrienmoi_tonghop";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vloaitb_id", OracleType.VarChar).Value = vloaitb_id;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;
		objCmd.Parameters.Add("vkieu", OracleType.VarChar).Value = vkieu;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable Lay_ds_tamdung_tonghop(string vtungay, string vdenngay, string vloaitb_id, string vdonvi_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.Lay_ds_tamdung_tonghop";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vloaitb_id", OracleType.VarChar).Value = vloaitb_id;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable Lay_ds_thanhly_tonghop(string vtungay, string vdenngay, string vloaitb_id, string vdonvi_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.Lay_ds_thanhly_tonghop";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vloaitb_id", OracleType.VarChar).Value = vloaitb_id;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable thongke_cskh_tonghop(string vtungay, string vdenngay,string vloaitb_id, string vdonvi_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.thongke_cskh_tonghop";
        objCmd.CommandType = CommandType.StoredProcedure;
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vloaitb_id", OracleType.VarChar).Value = vloaitb_id;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }
	
	
    [WebMethod]
    public DataTable thongke_cskh_tonghop_full(string vtungay, string vdenngay, string vdonvi_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.thongke_cskh_tonghop_full";
        objCmd.CommandType = CommandType.StoredProcedure;
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }	
	
	
	

    [WebMethod]
    public DataTable HUNGPT_BAOHONG_TH(string vtungay, string vdenngay, string vloaitb_id, string vdonvi_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.HUNGPT_BAOHONG_TH";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vloaitb_id", OracleType.VarChar).Value = vloaitb_id;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable HUNGPT_KETCUOIDOITHU_TH(string vtungay, string vdenngay, string vdonvi_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.HUNGPT_KETCUOIDOITHU_TH";
        objCmd.CommandType = CommandType.StoredProcedure;
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable DanhSachTrungTam(string donvi_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = " select trungtam_id,ten_trungtam from tinhcuoc.qt_trungtam where trungtam_id ='" + donvi_id + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;

    }


    [WebMethod]
    public DataTable DanhSachTrungTam_ALL()
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select 6 trungtam_id,'Tất cả trung tâm'ten_trungtam from dual  UNION ALL select trungtam_id,ten_trungtam from tinhcuoc.qt_trungtam";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable DanhSachDiaBan(string trungtam_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = " select distinct a.khuvuc_id,a.ten_kv || '-' || c.ten_nv ten_kv from css_hyn.khuvuc a, css_hyn.dbtb_kv b, admin_hyn.nhanvien c, css_hyn.khuvuc_nv d  where a.khuvuc_id = b.khuvuc_id and a.hyn_trungtam = '" + trungtam_id + "'  and c.nhanvien_id = d.nhanvien_id and a.khuvuc_id = d.khuvuc_id and d.loainv_id=51 order by a.khuvuc_id";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable Lay_ds_tamdung_diaban(string vtungay, string vdenngay, string vloaitb_id, string vdonvi_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.Lay_ds_tamdung_diaban";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vloaitb_id", OracleType.VarChar).Value = vloaitb_id;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable Lay_ds_thanhly_diaban(string vtungay, string vdenngay, string vloaitb_id, string vdonvi_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.Lay_ds_thanhly_diaban";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vloaitb_id", OracleType.VarChar).Value = vloaitb_id;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable thongke_cskh_diaban(string vtungay, string vdenngay, string vloaitb_id, string vdonvi_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.thongke_cskh_diaban";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vloaitb_id", OracleType.VarChar).Value = vloaitb_id;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }
	
	
    [WebMethod]
    public DataTable thongke_cskh_diaban_full(string vtungay, string vdenngay, string vdonvi_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.thongke_cskh_diaban_full";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }	
	
	

    [WebMethod]
    public DataTable baohong_theo_diaban(string vtungay, string vdenngay, string vloaitb_id, string vdonvi_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.baohong_theo_diaban";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vloaitb_id", OracleType.VarChar).Value = vloaitb_id;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable cabman_theo_diaban(string vtungay, string vdenngay, string vdonvi_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.cabman_theo_diaban";
        objCmd.CommandType = CommandType.StoredProcedure;
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable tonghop_ds_thuhoi()
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.tonghop_ds_thuhoi";
        objCmd.CommandType = CommandType.StoredProcedure;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }


    #region ver2

    //danh sách phát triển mới tổng hợp + đã xử lý
    [WebMethod]
    public DataTable Lay_ds_phattrienmoi_dxl(string vtungay, string vdenngay, string vloaitb_id, string vdonvi_id,string vkieu)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.Lay_ds_phattrienmoi_dxl";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vloaitb_id", OracleType.VarChar).Value = vloaitb_id;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;
		objCmd.Parameters.Add("vkieu", OracleType.VarChar).Value = vkieu;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable Lay_ds_phattrienmoi_dxl_diaban(string vtungay, string vdenngay, string vloaitb_id, string vdonvi_id,string vkieu)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.Lay_ds_phattrienmoi_dxl_diaban";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vloaitb_id", OracleType.VarChar).Value = vloaitb_id;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;
		objCmd.Parameters.Add("vkieu", OracleType.VarChar).Value = vkieu;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    //danh sách báo hỏng tổng hợp + đã xử lý
    [WebMethod]
    public DataTable tonghop_ds_baohong_dxl(string vtungay, string vdenngay, string vloaitb_id, string vdonvi_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.tonghop_ds_baohong_dxl";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vloaitb_id", OracleType.VarChar).Value = vloaitb_id;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable baohong_theo_diaban_dxl(string vtungay, string vdenngay, string vloaitb_id, string vdonvi_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.baohong_theo_diaban_dxl";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vloaitb_id", OracleType.VarChar).Value = vloaitb_id;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }


    //danh sách báo hỏng lặp lai tổng hợp + đã xử lý
    [WebMethod]
    public DataTable tonghop_ds_bhll(string vtungay, string vdenngay, string vdonvi_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.tonghop_ds_bhll";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable tonghop_ds_bhll_diaban(string vtungay, string vdenngay, string vdonvi_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.tonghop_ds_bhll_diaban";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }



    //danh sách hài lòng, không hài lòng tổng hợp + đã xử lý
    [WebMethod]
    public DataTable tonghop_ds_dohailong_ptm(string vtungay, string vdenngay, string vdonvi_id,string vloaitb_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.tonghop_ds_dohailong_ptm";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;
        objCmd.Parameters.Add("vloaitb_id", OracleType.VarChar).Value = vloaitb_id;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }
	
	
	
	    //danh sách hài lòng, không hài lòng tổng hợp + đã xử lý
    [WebMethod]
    public DataTable dohailong_ptm_new_all_TT(string vtungay, string vdenngay)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.dohailong_ptm_new_all_TT";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }
	
	
    [WebMethod]
    public DataTable dohailong_bh_new_all_TT(string vtungay, string vdenngay)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.dohailong_bh_new_all_TT";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }	
	
	
	
	
	
	
	
	
    [WebMethod]
    public DataTable tonghop_ds_dohailong_bh(string vtungay, string vdenngay, string vdonvi_id,string vloaitb_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.tonghop_ds_dohailong_bh";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;
        objCmd.Parameters.Add("vloaitb_id", OracleType.VarChar).Value = vloaitb_id;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }	
	
	
	
	
	
    [WebMethod]
    public DataTable tonghop_ds_dohailong_diaban_ptm(string vtungay, string vdenngay, string vdonvi_id,string vloaitb_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.tonghop_ds_dohailong_diaban_ptm";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;
        objCmd.Parameters.Add("vloaitb_id", OracleType.VarChar).Value = vloaitb_id;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }
	
	
    [WebMethod]
    public DataTable tonghop_ds_dohailong_diaban_bh(string vtungay, string vdenngay, string vdonvi_id,string vloaitb_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.tonghop_ds_dohailong_diaban_bh";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vdonvi_id", OracleType.VarChar).Value = vdonvi_id;
        objCmd.Parameters.Add("vloaitb_id", OracleType.VarChar).Value = vloaitb_id;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }	
	
	
	
	
	
	


    //danh sách nguy cơ mất liên lạc
    [WebMethod]
    public DataTable DanhSachThuebaoNguyCoMLL_TrungTam(string vtungay_ncmll, string vdenngay_ncmll,string vtungay, string vdenngay, string vtrungtam_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.DanhSachThuebaoNguyCoMLL_TrungTam";
        objCmd.CommandType = CommandType.StoredProcedure;
        objCmd.Parameters.Add("vtungay_ncmll", OracleType.VarChar).Value = vtungay_ncmll;
        objCmd.Parameters.Add("vdenngay_ncmll", OracleType.VarChar).Value = vdenngay_ncmll;
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vtrungtam_id", OracleType.VarChar).Value = vtrungtam_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable DanhSachThuebaoNguyCoMLL_DiaBan(string vtungay_ncmll, string vdenngay_ncmll,string vtungay, string vdenngay, string vtrungtam_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.DanhSachThuebaoNguyCoMLL_DiaBan";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
		objCmd.Parameters.Add("vtungay_ncmll", OracleType.VarChar).Value = vtungay_ncmll;
        objCmd.Parameters.Add("vdenngay_ncmll", OracleType.VarChar).Value = vdenngay_ncmll;
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vtrungtam_id", OracleType.VarChar).Value = vtrungtam_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    #endregion


    [WebMethod]
    public DataTable LoadKV(string trungtam_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = " select a.khuvuc_id, a.ten_kv || '-' || c.ten_nv ten_kv  from css_hyn.khuvuc a, css_hyn.khuvuc_nv b, admin_hyn.nhanvien c where a.khuvuc_id = b.khuvuc_id and b.nhanvien_id = c.nhanvien_id and b.loainv_id = 51 and hyn_trungtam= '" + trungtam_id + "' order by khuvuc_id";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable thoigian_ht_ptm(string vtungay, string vdenngay, string vkhuvuc_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.thoigian_ht_ptm";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vkhuvuc_id", OracleType.VarChar).Value = vkhuvuc_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable thoigian_ht_bh(string vtungay, string vdenngay, string vkhuvuc_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.thoigian_ht_bh";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vkhuvuc_id", OracleType.VarChar).Value = vkhuvuc_id;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable thoigian_ht_thuhoi(string vtungay, string vdenngay, string vkhuvuc_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.thoigian_ht_thuhoi";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vkhuvuc_id", OracleType.VarChar).Value = vkhuvuc_id;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable thoigian_ht_cskh(string vtungay, string vdenngay, string vkhuvuc_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.thoigian_ht_cskh";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vkhuvuc_id", OracleType.VarChar).Value = vkhuvuc_id;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable tonghop_ht_ptm(string vtungay, string vdenngay, string vkhuvuc_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.tonghop_ht_ptm";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vkhuvuc_id", OracleType.VarChar).Value = vkhuvuc_id;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable tonghop_ht_baohong(string vtungay, string vdenngay, string vkhuvuc_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.tonghop_ht_baohong";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vkhuvuc_id", OracleType.VarChar).Value = vkhuvuc_id;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable tonghop_ht_thuhoi(string vtungay, string vdenngay, string vkhuvuc_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.tonghop_ht_thuhoi";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vkhuvuc_id", OracleType.VarChar).Value = vkhuvuc_id;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable tonghop_ht_cskh(string vtungay, string vdenngay, string vkhuvuc_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.tonghop_ht_cskh";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vkhuvuc_id", OracleType.VarChar).Value = vkhuvuc_id;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable chitiet_ht_ptm(string vtungay, string vdenngay, string vkhuvuc_id, string vgio)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.chitiet_ht_ptm";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vkhuvuc_id", OracleType.VarChar).Value = vkhuvuc_id;
        objCmd.Parameters.Add("vgio", OracleType.VarChar).Value = vgio;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable chitiet_ht_baohong(string vtungay, string vdenngay, string vkhuvuc_id, string vgio)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.chitiet_ht_baohong";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vkhuvuc_id", OracleType.VarChar).Value = vkhuvuc_id;
        objCmd.Parameters.Add("vgio", OracleType.VarChar).Value = vgio;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable chitiet_ht_thuhoi(string vtungay, string vdenngay, string vkhuvuc_id, string vgio)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.chitiet_ht_thuhoi";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vkhuvuc_id", OracleType.VarChar).Value = vkhuvuc_id;
        objCmd.Parameters.Add("vgio", OracleType.VarChar).Value = vgio;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable chitiet_ht_cskh(string vtungay, string vdenngay, string vkhuvuc_id, string vgio)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.chitiet_ht_cskh";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vkhuvuc_id", OracleType.VarChar).Value = vkhuvuc_id;
        objCmd.Parameters.Add("vgio", OracleType.VarChar).Value = vgio;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public void CapNhat_OTP(string otp, string acc)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "Update dashboard_nguoidung SET OTP='" + otp + "' where MA_ND  = '" + acc + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        cmd.ExecuteNonQuery();
        conn2.Close();
    }


    [WebMethod]
    public DataTable KiemTra_NguoiDung(string acc)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = " select * from dashboard_nguoidung where ma_nd='" + acc + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        conn2.Close();
        return dt;

    }


    //maps, danh sách cắt hẳn, tạm dừng
    [WebMethod]
    public DataTable data()
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select  ma_nd,ten_nd,substr(toado,0,instr(toado,'-',1) -1) lat, substr(toado,instr(toado,'-',1)+1,length(toado)) lng,ngay  from ( select c.ma_nd, c.ten_nd,b.key_parameter toado,a.ngay from ( select user_name,max(datetime) ngay from ADMIN_HYN.Log_Action where Action_Name='CURRENT_LOCATION' group by user_name ) a,  ADMIN_HYN.Log_Action b,admin_HYN.nguoidung c where a.ngay = b.datetime and a.user_name = b.user_name and b.Action_Name='CURRENT_LOCATION' and a.user_name =c.ma_nd ) where to_char(ngay,'YYYYMMDD')=to_char(sysdate,'YYYYMMDD')";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable timkiem_nguoi(string ma_nd, string ngay)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select  ma_nd,ten_nd,substr(toado,0,instr(toado,'-',1) -1) lat, substr(toado,instr(toado,'-',1)+1,length(toado)) lng,ngay,'http://maps.google.com/mapfiles/ms/micons/blue-dot.png' icon  from ( select b.ma_nd, b.ten_nd,a.key_parameter toado,a.datetime ngay from ADMIN_HYN.Log_Action a, admin_HYN.nguoidung b where a.Action_Name='CURRENT_LOCATION' and a.user_name =b.ma_nd ) where ma_nd='" + ma_nd + "' and to_char(ngay,'YYYYMMDD')='" + ngay + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable tkkythuat(string donvi_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select a.* from ( select  ma_nd,ten_nd,substr(toado,0,instr(toado,'-',1) -1) lat, substr(toado,instr(toado,'-',1)+1,length(toado)) lng,ngay,'http://maps.google.com/mapfiles/ms/micons/blue-dot.png' icon   from ( select c.ma_nd, c.ten_nd,b.key_parameter toado,a.ngay from ( select user_name,max(datetime) ngay from ADMIN_HYN.Log_Action  where Action_Name='CURRENT_LOCATION' group by user_name ) a,  ADMIN_HYN.Log_Action b,admin_HYN.nguoidung c  where a.ngay = b.datetime and a.user_name = b.user_name and b.Action_Name='CURRENT_LOCATION' and a.user_name =c.ma_nd ) ) a, admin_hyn.nguoidung b, admin_hyn.nhanvien c, admin_hyn.nhanvien_lnv d where b.nhanvien_id = c.nhanvien_id and b.nhanvien_id = d.nhanvien_id and a.ma_nd= b.ma_nd and d.loainv_id=51 and to_char(a.ngay,'YYYYMMDD')=to_char(sysdate,'YYYYMMDD') and c.donvi_dl_id in (" + donvi_id + ")";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable tkkinhdoanh(string donvi_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select a.* from ( select  ma_nd,ten_nd,substr(toado,0,instr(toado,'-',1) -1) lat, substr(toado,instr(toado,'-',1)+1,length(toado)) lng,ngay   from ( select c.ma_nd, c.ten_nd,b.key_parameter toado,a.ngay,'http://maps.google.com/mapfiles/ms/micons/blue-dot.png' icon from ( select user_name,max(datetime) ngay from ADMIN_HYN.Log_Action  where Action_Name='CURRENT_LOCATION' group by user_name ) a,  ADMIN_HYN.Log_Action b,admin_HYN.nguoidung c  where a.ngay = b.datetime and a.user_name = b.user_name and b.Action_Name='CURRENT_LOCATION' and a.user_name =c.ma_nd ) ) a, admin_hyn.nguoidung b, admin_hyn.nhanvien c, admin_hyn.nhanvien_lnv d where b.nhanvien_id = c.nhanvien_id and b.nhanvien_id = d.nhanvien_id and a.ma_nd= b.ma_nd and d.loainv_id=52 and to_char(a.ngay,'YYYYMMDD')=to_char(sysdate,'YYYYMMDD') and c.donvi_dl_id in (" + donvi_id + ")";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable timkiem_donvi(string donvi_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select a.ma_nd,a.ten_nd,a.lat,a.lng,a.ngay,b.link icon from ( select RANK() OVER ( PARTITION BY 1 ORDER BY nguoidung_id DESC ) id,ma_nd,ten_nd,substr(toado,0,instr(toado,'-',1) -1) lat, substr(toado,instr(toado,'-',1)+1,length(toado)) lng, ngay  from ( select b.ma_nd, b.ten_nd,a.datetime ngay, a.key_parameter toado, b.nguoidung_id from ADMIN_HYN.Log_Action a, admin_HYN.nguoidung b , admin_hyn.nhanvien c, admin_hyn.nhanvien_lnv d where a.Action_Name='CURRENT_LOCATION' and a.user_name =b.ma_nd and b.nhanvien_id = c.nhanvien_id and b.nhanvien_id = d.nhanvien_id  and d.loainv_id=47 and to_char(a.datetime,'YYYYMMDD')=to_char(sysdate,'YYYYMMDD')  and c.donvi_dl_id in (" + donvi_id + ") )  ) a, icon_googlemap b where a.id=b.id";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable hienthi_line()
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select a.ma_nd,a.ten_nd,a.lat,a.lng,a.ngay,b.link icon from ( select RANK() OVER ( PARTITION BY 1 ORDER BY nguoidung_id DESC ) id,ma_nd,ten_nd,substr(toado,0,instr(toado,'-',1) -1) lat, substr(toado,instr(toado,'-',1)+1,length(toado)) lng, ngay  from ( select b.ma_nd, b.ten_nd,a.datetime ngay, a.key_parameter toado, b.nguoidung_id from ADMIN_HYN.Log_Action a, admin_HYN.nguoidung b , admin_hyn.nhanvien c, admin_hyn.nhanvien_lnv d where a.Action_Name='CURRENT_LOCATION' and a.user_name =b.ma_nd and b.nhanvien_id = c.nhanvien_id and b.nhanvien_id = d.nhanvien_id  and d.loainv_id=47 and to_char(a.datetime,'YYYYMMDD')='20190123'  and c.donvi_dl_id in (1,2,3,4,5,6,7,8,9,10) )  ) a, icon_googlemap b where a.id=b.id";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        conn2.Close();
        return dt;
    }
    [WebMethod]
    public DataTable hienthidanhsach_tamdung_cathuy(string ngaybd, string ngaykt, string loaihinhtb, string donvi_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select * from ( select a.ma_tb ma_nd,a.ten_tb ten_nd,trim(a.vido_ld) lat, trim(a.kinhdo_ld) lng,a.ngay,b.link icon,a.ghichu from ( select a.ma_tb,a.ten_tb,b.kinhdo_ld, b.vido_ld,a.ngay_ht ngay,'tamdung' ghichu from css_hyn.hd_thuebao a, css_hyn.diachi_tb b where a.thuebao_id = b.thuebao_id and a.hdtb_id in ( select hdtb_id from css_hyn.dangky_dvgt a where hdtb_id in (select hdtb_id from css_hyn.hd_thuebao a where kieuld_id in (39,128) and TO_CHAR (a.ngay_ht, 'yyyyMMdd') >= '" + ngaybd + "' and TO_CHAR (a.ngay_ht, 'yyyyMMdd') <= '" + ngaykt + "' and a.tthd_id in (6) and a.loaitb_id= '" + loaihinhtb + "' and a.donvi_id in (" + donvi_id + ")  ) and a.kieu_yc=1 ) and b.kinhdo_ld is not null and b.vido_ld is not null  and b.kinhdo_ld not in ('0') and b.vido_ld not in ('0') UNION ALL SELECT a.ma_tb,a.ten_tb,d.kinhdo_ld, d.vido_ld,a.ngay_ht ngay,'thanhly' ghichu FROM    css_hyn.hd_thuebao a, css_hyn.hd_khachhang c,css_hyn.diachi_tb d WHERE   a.hdkh_id = c.hdkh_id and a.tthd_id in (6) and a.loaitb_id in (" + loaihinhtb + ") and c.loaihd_id = 4  and TO_CHAR (a.ngay_ht, 'yyyyMMdd') >= '" + ngaybd + "'   and TO_CHAR (a.ngay_ht, 'yyyyMMdd') <= '" + ngaykt + "' and a.donvi_id in (" + donvi_id + ") and a.thuebao_id = d.thuebao_id and d.kinhdo_ld is not null and d.vido_ld is not null UNION ALL select ma_tb,ten_tb,decode(kinhdo_ld,0,(106.0348942+106.0348942*rownum/100000),kinhdo_ld) kinhdo_ld, decode(vido_ld,0,(20.6582938+20.6582938*rownum/100000),vido_ld) vido_ld,ngay,'ptm_cdoi_k_toado' ghichu from (select ma_tb,ten_tb,kinhdo_ld, vido_ld, ngay from ( SELECT a.ma_tb,a.ten_tb,d.kinhdo_ld, d.vido_ld,a.ngay_ht ngay FROM    css_hyn.hd_thuebao a, css_hyn.hd_khachhang c,css_hyn.diachi_tb d WHERE   a.hdkh_id = c.hdkh_id and a.tthd_id in (6) and a.loaitb_id in (" + loaihinhtb + ") and c.loaihd_id = 1  and TO_CHAR (a.ngay_ins, 'yyyyMMdd') >= '" + ngaybd + "'   and TO_CHAR (a.ngay_ins, 'yyyyMMdd') <= '" + ngaykt + "' and a.donvi_id in (" + donvi_id + ") and a.thuebao_id = d.thuebao_id and d.kinhdo_ld is not null and d.vido_ld is not null UNION ALL SELECT a.ma_tb,a.ten_tb,d.kinhdo_ld, d.vido_ld,a.ngay_ht ngay FROM    css_hyn.hd_thuebao a, css_hyn.hd_khachhang c,css_hyn.diachi_tb d WHERE   a.hdkh_id = c.hdkh_id and a.tthd_id in (6) and a.loaitb_id in (" + loaihinhtb + ") and c.loaihd_id = 6  and TO_CHAR (a.ngay_ins, 'yyyyMMdd') >= '" + ngaybd + "'   and TO_CHAR (a.ngay_ins, 'yyyyMMdd') <= '" + ngaykt + "' and a.donvi_id in (" + donvi_id + ") and a.thuebao_id = d.thuebao_id and a.kieuld_id not in (16,22)  and d.kinhdo_ld is not null and d.vido_ld is not null  ) where kinhdo_ld  in (0) and vido_ld  in (0) ) UNION ALL select ma_tb,ten_tb,decode(kinhdo_ld,0,106.0348942,kinhdo_ld) kinhdo_ld, decode(vido_ld,0,20.6582938,vido_ld) vido_ld,ngay,'ptm_cdoi_co_toado' ghichu from (select ma_tb,ten_tb,kinhdo_ld, vido_ld, ngay from (SELECT a.ma_tb,a.ten_tb,d.kinhdo_ld, d.vido_ld,a.ngay_ht ngay FROM    css_hyn.hd_thuebao a, css_hyn.hd_khachhang c,css_hyn.diachi_tb d WHERE   a.hdkh_id = c.hdkh_id and a.tthd_id in (6) and a.loaitb_id in (" + loaihinhtb + ") and c.loaihd_id = 1  and TO_CHAR (a.ngay_ins, 'yyyyMMdd') >= '" + ngaybd + "'   and TO_CHAR (a.ngay_ins, 'yyyyMMdd') <= '" + ngaykt + "' and a.donvi_id in (" + donvi_id + ") and a.thuebao_id = d.thuebao_id and d.kinhdo_ld is not null and d.vido_ld is not null UNION ALL SELECT a.ma_tb,a.ten_tb,d.kinhdo_ld, d.vido_ld,a.ngay_ht ngay FROM    css_hyn.hd_thuebao a, css_hyn.hd_khachhang c,css_hyn.diachi_tb d WHERE   a.hdkh_id = c.hdkh_id and a.tthd_id in (6) and a.loaitb_id in (" + loaihinhtb + ") and c.loaihd_id = 6  and TO_CHAR (a.ngay_ins, 'yyyyMMdd') >= '" + ngaybd + "'   and TO_CHAR (a.ngay_ins, 'yyyyMMdd') <= '" + ngaykt + "' and a.donvi_id in (" + donvi_id + ") and a.thuebao_id = d.thuebao_id and a.kieuld_id not in (16,22)  and d.kinhdo_ld is not null and d.vido_ld is not null  ) where kinhdo_ld not in (0) and vido_ld not in (0) ) ) a, icon_thongbao b where a.ghichu = b.ghichu ) where lat not in ('0') and lng not in ('0') order by icon ";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable tamdung(string ngaybd, string ngaykt, string loaihinhtb, string donvi_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select a.ma_tb,a.ten_tb,b.kinhdo_ld, b.vido_ld,a.ngay_ht ngay,'tamdung' ghichu from css_hyn.hd_thuebao a, css_hyn.diachi_tb b where a.thuebao_id = b.thuebao_id and a.hdtb_id in ( select hdtb_id from css_hyn.dangky_dvgt a where hdtb_id in (select hdtb_id from css_hyn.hd_thuebao a where kieuld_id in (39,128) and TO_CHAR (a.ngay_ht, 'yyyyMMdd') >= '" + ngaybd + "' and TO_CHAR (a.ngay_ht, 'yyyyMMdd') <= '" + ngaykt + "' and a.tthd_id in (6) and a.loaitb_id= '" + loaihinhtb + "' and a.donvi_id in (" + donvi_id + ")  ) and a.kieu_yc=1 ) and b.kinhdo_ld is not null and b.vido_ld is not null  and b.kinhdo_ld not in ('0') and b.vido_ld not in ('0')";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable thanhly(string ngaybd, string ngaykt, string loaihinhtb, string donvi_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "SELECT a.ma_tb,a.ten_tb,d.kinhdo_ld, d.vido_ld,a.ngay_ht ngay,'thanhly' ghichu FROM    css_hyn.hd_thuebao a, css_hyn.hd_khachhang c,css_hyn.diachi_tb d WHERE   a.hdkh_id = c.hdkh_id and a.tthd_id in (6) and a.loaitb_id in (" + loaihinhtb + ") and c.loaihd_id = 4  and TO_CHAR (a.ngay_ht, 'yyyyMMdd') >= '" + ngaybd + "'    and TO_CHAR (a.ngay_ht, 'yyyyMMdd') <= '" + ngaykt + "'  and a.donvi_id in (" + donvi_id + ") and a.thuebao_id = d.thuebao_id and d.kinhdo_ld is not null and d.vido_ld is not null and d.kinhdo_ld not in ('0') and d.vido_ld not in ('0')";

        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable ptm_co_toado(string ngaybd, string ngaykt, string loaihinhtb, string donvi_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select ma_tb,ten_tb,decode(kinhdo_ld,0,106.0348942,kinhdo_ld) kinhdo_ld, decode(vido_ld,0,20.6582938,vido_ld) vido_ld,ngay,'ptm_cdoi_co_toado' ghichu from ( select ma_tb,ten_tb,kinhdo_ld, vido_ld, ngay from ( SELECT a.ma_tb,a.ten_tb,d.kinhdo_ld, d.vido_ld,a.ngay_ht ngay FROM     css_hyn.hd_thuebao a, css_hyn.hd_khachhang c,css_hyn.diachi_tb d WHERE    a.hdkh_id = c.hdkh_id and a.tthd_id in (6) and a.loaitb_id in (" + loaihinhtb + ") and c.loaihd_id = 1   and TO_CHAR (a.ngay_ins, 'yyyyMMdd') >= '" + ngaybd + "'    and TO_CHAR (a.ngay_ins, 'yyyyMMdd') <= '" + ngaykt + "'  and a.donvi_id in (" + donvi_id + ")  and a.thuebao_id = d.thuebao_id  and d.kinhdo_ld is not null and d.vido_ld is not null  UNION ALL  SELECT a.ma_tb,a.ten_tb,d.kinhdo_ld, d.vido_ld,a.ngay_ht ngay FROM    css_hyn.hd_thuebao a, css_hyn.hd_khachhang c,css_hyn.diachi_tb d WHERE   a.hdkh_id = c.hdkh_id and a.tthd_id in (6) and a.loaitb_id in (" + loaihinhtb + ") and c.loaihd_id = 6  and TO_CHAR (a.ngay_ins, 'yyyyMMdd') >= '" + ngaybd + "'   and TO_CHAR (a.ngay_ins, 'yyyyMMdd') <= '" + ngaykt + "' and a.donvi_id in (" + donvi_id + ") and a.thuebao_id = d.thuebao_id and a.kieuld_id not in (16,22)   and d.kinhdo_ld is not null and d.vido_ld is not null  ) where kinhdo_ld not in (0) and vido_ld not in (0) )";

        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable ptm_k_toado(string ngaybd, string ngaykt, string loaihinhtb, string donvi_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select ma_tb,ten_tb,decode(kinhdo_ld,0,106.0348942,kinhdo_ld) kinhdo_ld, decode(vido_ld,0,20.6582938,vido_ld) vido_ld,ngay,'ptm_cdoi_co_toado' ghichu from ( select ma_tb,ten_tb,kinhdo_ld, vido_ld, ngay from ( SELECT a.ma_tb,a.ten_tb,d.kinhdo_ld, d.vido_ld,a.ngay_ht ngay FROM     css_hyn.hd_thuebao a, css_hyn.hd_khachhang c,css_hyn.diachi_tb d WHERE    a.hdkh_id = c.hdkh_id and a.tthd_id in (6) and a.loaitb_id in (" + loaihinhtb + ") and c.loaihd_id = 1   and TO_CHAR (a.ngay_ins, 'yyyyMMdd') >= '" + ngaybd + "'    and TO_CHAR (a.ngay_ins, 'yyyyMMdd') <= '" + ngaykt + "'  and a.donvi_id in (" + donvi_id + ")  and a.thuebao_id = d.thuebao_id  and d.kinhdo_ld is not null and d.vido_ld is not null  UNION ALL  SELECT a.ma_tb,a.ten_tb,d.kinhdo_ld, d.vido_ld,a.ngay_ht ngay FROM    css_hyn.hd_thuebao a, css_hyn.hd_khachhang c,css_hyn.diachi_tb d WHERE   a.hdkh_id = c.hdkh_id and a.tthd_id in (6) and a.loaitb_id in (" + loaihinhtb + ") and c.loaihd_id = 6  and TO_CHAR (a.ngay_ins, 'yyyyMMdd') >= '" + ngaybd + "'   and TO_CHAR (a.ngay_ins, 'yyyyMMdd') <= '" + ngaykt + "' and a.donvi_id in (" + donvi_id + ") and a.thuebao_id = d.thuebao_id and a.kieuld_id not in (16,22)   and d.kinhdo_ld is not null and d.vido_ld is not null  ) where kinhdo_ld  in (0) and vido_ld in (0) )";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable tonghop(string ngaybd, string ngaykt, string loaihinhtb, string donvi_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select * from ( select a.ma_tb ma_nd,a.ten_tb ten_nd,trim(a.vido_ld) lat, trim(a.kinhdo_ld) lng,a.ngay,b.link icon,a.ghichu from ( select a.ma_tb,a.ten_tb,b.kinhdo_ld, b.vido_ld,a.ngay_ht ngay,'tamdung' ghichu from css_hyn.hd_thuebao a, css_hyn.diachi_tb b where a.thuebao_id = b.thuebao_id and a.hdtb_id in ( select hdtb_id from css_hyn.dangky_dvgt a where hdtb_id in (select hdtb_id from css_hyn.hd_thuebao a where kieuld_id in (39,128) and TO_CHAR (a.ngay_ht, 'yyyyMMdd') >= '" + ngaybd + "' and TO_CHAR (a.ngay_ht, 'yyyyMMdd') <= '" + ngaykt + "' and a.tthd_id in (6) and a.loaitb_id= '" + loaihinhtb + "' and a.donvi_id in (" + donvi_id + ")  ) and a.kieu_yc=1 ) and b.kinhdo_ld is not null and b.vido_ld is not null  and b.kinhdo_ld not in ('0') and b.vido_ld not in ('0') UNION ALL SELECT a.ma_tb,a.ten_tb,d.kinhdo_ld, d.vido_ld,a.ngay_ht ngay,'thanhly' ghichu FROM    css_hyn.hd_thuebao a, css_hyn.hd_khachhang c,css_hyn.diachi_tb d WHERE   a.hdkh_id = c.hdkh_id and a.tthd_id in (6) and a.loaitb_id in (" + loaihinhtb + ") and c.loaihd_id = 4  and TO_CHAR (a.ngay_ht, 'yyyyMMdd') >= '" + ngaybd + "'   and TO_CHAR (a.ngay_ht, 'yyyyMMdd') <= '" + ngaykt + "' and a.donvi_id in (" + donvi_id + ") and a.thuebao_id = d.thuebao_id and d.kinhdo_ld is not null and d.vido_ld is not null  UNION ALL SELECT a.ma_tb,a.ten_tb,d.kinhdo_ld, d.vido_ld,a.ngay_ht ngay,'ptm_cdoi' ghichu FROM    css_hyn.hd_thuebao a, css_hyn.hd_khachhang c,css_hyn.diachi_tb d WHERE   a.hdkh_id = c.hdkh_id and a.tthd_id in (6) and a.loaitb_id in (" + loaihinhtb + ") and c.loaihd_id = 1  and TO_CHAR (a.ngay_ht, 'yyyyMMdd') >= '" + ngaybd + "'   and TO_CHAR (a.ngay_ht, 'yyyyMMdd') <= '" + ngaykt + "' and a.donvi_id in (" + donvi_id + ") and a.thuebao_id = d.thuebao_id and d.kinhdo_ld is not null and d.vido_ld is not null UNION ALL SELECT a.ma_tb,a.ten_tb,d.kinhdo_ld, d.vido_ld,a.ngay_ht ngay,'ptm_cdoi' ghichu FROM    css_hyn.hd_thuebao a, css_hyn.hd_khachhang c,css_hyn.diachi_tb d WHERE   a.hdkh_id = c.hdkh_id and a.tthd_id in (6) and a.loaitb_id in (" + loaihinhtb + ") and c.loaihd_id = 6  and TO_CHAR (a.ngay_ht, 'yyyyMMdd') >= '" + ngaybd + "'   and TO_CHAR (a.ngay_ht, 'yyyyMMdd') <= '" + ngaykt + "' and a.donvi_id in (" + donvi_id + ") and a.thuebao_id = d.thuebao_id and a.kieuld_id not in (16,22)  and d.kinhdo_ld is not null and d.vido_ld is not null ) a, icon_thongbao b where a.ghichu = b.ghichu  ) where lat not in ('0') and lng not in ('0') order by icon";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        conn2.Close();
        return dt;
    }




    [WebMethod]
    public DataTable toado_nd(string donvi_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select a.ma_nd,a.ten_nd,a.lat,a.lng,a.ngay,b.link icon from ( select RANK() OVER ( PARTITION BY 1 ORDER BY nguoidung_id DESC ) id,ma_nd,ten_nd,substr(toado,0,instr(toado,'-',1) -1) lat, substr(toado,instr(toado,'-',1)+1,length(toado)) lng, ngay  from ( select b.ma_nd, b.ten_nd,a.datetime ngay, a.key_parameter toado, b.nguoidung_id from ADMIN_HYN.Log_Action a, admin_HYN.nguoidung b , admin_hyn.nhanvien c, admin_hyn.nhanvien_lnv d where a.Action_Name='CURRENT_LOCATION' and a.user_name =b.ma_nd and b.nhanvien_id = c.nhanvien_id and b.nhanvien_id = d.nhanvien_id  and d.loainv_id=47 and to_char(a.datetime,'YYYYMMDD')=to_char(sysdate,'YYYYMMDD')  and c.donvi_dl_id in (" + donvi_id + ") )  ) a, icon_googlemap b where a.id=b.id";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable timkiem_nd(string donvi)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select b.* from admin_hyn.nhanvien a, admin_hyn.nguoidung b, admin_hyn.nhanvien_lnv c where a.nhanvien_id=b.nhanvien_id and b.nhanvien_id= c.nhanvien_id and c.loainv_id=47 and a.donvi_dl_id in (" + donvi + ") and b.trangthai=1";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable hienthi_toado_nd(string ma_nd, string ngay)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "select  ma_nd,ten_nd,substr(toado,0,instr(toado,'-',1) -1) lat, substr(toado,instr(toado,'-',1)+1,length(toado)) lng,ngay  from ( select b.ma_nd, b.ten_nd,a.key_parameter toado,a.datetime ngay from ADMIN_HYN.Log_Action a, admin_HYN.nguoidung b where a.Action_Name='CURRENT_LOCATION' and a.user_name =b.ma_nd ) where ma_nd='" + ma_nd + "' and to_char(ngay,'YYYYMMDD')='" + ngay + "'";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        conn2.Close();
        return dt;
    }
	
    [WebMethod]
    public DataTable dtnguoidung(string ma_nd, string ngay)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
            string sql = "select  ma_nd,ten_nd,substr(toado,0,instr(toado,'-',1) -1) lat, substr(toado,instr(toado,'-',1)+1,length(toado)) lng,ngay  from ( select b.ma_nd, b.ten_nd,a.key_parameter toado,a.datetime ngay from ADMIN_HYN.Log_Action a, admin_HYN.nguoidung b where a.Action_Name='CURRENT_LOCATION' and a.user_name =b.ma_nd ) where ma_nd='" + ma_nd + "' and to_char(ngay,'YYYYMMDD')='" + ngay + "' order by ngay";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        conn2.Close();
        return dt;
    }	
	
    #endregion


    #region dokiem thuebao MLL
    [WebMethod]
    public string dokiem(string ip_slot)
    {
        string kqua = "0";
        dokiem.Services ws = new dokiem.Services();

        dokiem.MobileTestData dulieu = new dokiem.MobileTestData();
        dulieu = ws.TestPort("vietbt.hyn", "hyn123", ip_slot, 0);
        string down = dulieu.GPONTestResult.AttenuationDown;
        string up = dulieu.GPONTestResult.AttenuationUp;
		string tt = dulieu.GPONTestResult.OnuOperationState;
        if (Convert.ToDouble(up) < 28 && Convert.ToDouble(down) < 28)
        {
			if(tt == "ON")
			{
				kqua = "1";
			}
        }
        return kqua;
    }
    #endregion
	
	
	
	

    [WebMethod]
    public DataTable LayDanhSach_KhuVuc(string trungtam_id)
    {
        try
        {
			if (conn2.State == ConnectionState.Closed)
			{
				conn2.Open();
			}
			string sql = "select Ten_kv, khuvuc_id from css_hyn.khuvuc where hyn_trungtam= '"+trungtam_id+"' ";
			OracleCommand cmd = new OracleCommand(sql, conn2);
			OracleDataAdapter da = new OracleDataAdapter(cmd);
			DataSet ds = new DataSet();
			da.Fill(ds);
			DataTable dt = new DataTable();
			if (ds.Tables.Count > 0)
			{
				dt = ds.Tables[0];
			}
			conn2.Close();
			return dt;
        }
        catch
        {
            conn2.Close();
            return null;
        }
    }

    [WebMethod]
    public DataTable LayDuLieuBaoHong_KhuVuc(string tungay,string denngay,string khuvuc_id)
    {
        try
        {
			if (conn2.State == ConnectionState.Closed)
			{
				conn2.Open();
			}
			string sql = "select    a.thuebao_id   , b.ma_tb ,((a.ngay_bh- 7/24) - date '1970-01-01') * 86400000 as ngay_bh ,d.ten_kv,d.khuvuc_id ,((a.ngay_ht- 7/24) - date '1970-01-01') * 86400000 as ngay_ht  ,to_char(a.ngay_bh,'hh24.mi') h_bh ,to_char(a.ngay_ht,'hh24.mi') h_ht  from baohong_hyn.baohong a, css_hyn.db_thuebao b,css_hyn.dbtb_kv c, css_hyn.khuvuc d  where a.thuebao_id = b.thuebao_id and b.thuebao_id = c.thuebao_id and c.khuvuc_id=d.khuvuc_id  and a.ttbh_id=6 and to_char(a.ngay_ht,'YYYYMMDD')>= '"+tungay+"' and to_char(a.ngay_ht,'YYYYMMDD')<= '"+denngay+"' and d.khuvuc_id= '"+khuvuc_id+"'";
			OracleCommand cmd = new OracleCommand(sql, conn2);
			OracleDataAdapter da = new OracleDataAdapter(cmd);
			DataSet ds = new DataSet();
			da.Fill(ds);
			DataTable dt = new DataTable();
			if (ds.Tables.Count > 0)
			{
				dt = ds.Tables[0];
			}
			conn2.Close();
			return dt;
        }
        catch
        {
            conn2.Close();
            return null;
        }
    }



	[WebMethod]
    public int LuuLog(string taikhoan, string chuongtrinh)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }

        try
        {
            string sql = "insert into baocao_hyn.log_dangnhap values ('" + taikhoan + "',sysdate,'"+chuongtrinh+"')";
            OracleCommand cmd = new OracleCommand(sql, conn2);
            cmd = new OracleCommand(sql, conn2);
            cmd.ExecuteNonQuery();
            conn2.Close();
            return 1;//ok
        }
        catch
        {
            conn2.Close();
            return 0;//not ok
        }
    }






    [WebMethod]
    public DataTable dothihoa_spitter(string vhuyen,string vcap)
    {
            OracleCommand objCmd = new OracleCommand();
            objCmd.Connection = conn2;
            objCmd.CommandText = "baocao_hyn.duy_hyn.dothihoa_spitter";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("vhuyen", OracleType.VarChar).Value = vhuyen;
            objCmd.Parameters.Add("vcap", OracleType.VarChar).Value = vcap;
            objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
            DataTable dt = new DataTable();
            try
            {
                conn2.Open();
                objCmd.ExecuteNonQuery();
                OracleDataAdapter da = new OracleDataAdapter(objCmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception: {0}", ex.ToString());
            }
            conn2.Close();
            return dt;
    }


    [WebMethod]
    public DataTable hienthispliter_c1c2(string vport_id)
    {
            OracleCommand objCmd = new OracleCommand();
            objCmd.Connection = conn2;
            objCmd.CommandText = "baocao_hyn.duy_hyn.hienthispliter_c1c2";
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.Parameters.Add("vport_id", OracleType.VarChar).Value = vport_id;
            objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
            DataTable dt = new DataTable();
            try
            {
                conn2.Open();
                objCmd.ExecuteNonQuery();
                OracleDataAdapter da = new OracleDataAdapter(objCmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception: {0}", ex.ToString());
            }
            conn2.Close();
            return dt;
    }




    [WebMethod]
    public DataTable TONGHOP_THUHOI_TT(string vtungay_nocuoc, string vdenngay_nocuoc,string vtungay_tdtl, string vdenngay_tdtl)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.TONGHOP_THUHOI_TT";
        objCmd.CommandType = CommandType.StoredProcedure;
        objCmd.Parameters.Add("vtungay_nocuoc", OracleType.VarChar).Value = vtungay_nocuoc;
        objCmd.Parameters.Add("vdenngay_nocuoc", OracleType.VarChar).Value = vdenngay_nocuoc;
        objCmd.Parameters.Add("vtungay_tdtl", OracleType.VarChar).Value = vtungay_tdtl;
        objCmd.Parameters.Add("vdenngay_tdtl", OracleType.VarChar).Value = vdenngay_tdtl;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable TONGHOP_THUHOI_DIABAN(string vtungay_nocuoc, string vdenngay_nocuoc,string vtungay_tdtl, string vdenngay_tdtl,string vtrungtam)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.TONGHOP_THUHOI_DIABAN";
        objCmd.CommandType = CommandType.StoredProcedure;
        objCmd.Parameters.Add("vtungay_nocuoc", OracleType.VarChar).Value = vtungay_nocuoc;
        objCmd.Parameters.Add("vdenngay_nocuoc", OracleType.VarChar).Value = vdenngay_nocuoc;
        objCmd.Parameters.Add("vtungay_tdtl", OracleType.VarChar).Value = vtungay_tdtl;
        objCmd.Parameters.Add("vdenngay_tdtl", OracleType.VarChar).Value = vdenngay_tdtl;
		objCmd.Parameters.Add("vtrungtam", OracleType.VarChar).Value = vtrungtam;

        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }





    [WebMethod]
    public DataTable DanhSach_TramThietBi(string donvi_cha_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        try
        {
            string sql = "select a.* from admin_hyn.donvi a, admin_hyn.donvi_ldv b where a.donvi_id = b.donvi_id  and b.loaidv_id = 39 and a.donvi_cha_id = '"+donvi_cha_id+"' ";
            OracleCommand cmd = new OracleCommand(sql, conn2);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            conn2.Close();
            return dt;
        }
        catch
        {
            DataTable dtnull = new DataTable();
            conn2.Close();
            return dtnull;
        }
    }



    [WebMethod]
    public DataTable DanhSach_Dslam(string donvi_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        try
        {
            string sql = "select * from css_hyn.dslam where loai_dslam_id in (24,25) and donvi_id ='"+donvi_id+"'";
            OracleCommand cmd = new OracleCommand(sql, conn2);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            conn2.Close();
            return dt;
        }
        catch
        {
            DataTable dtnull = new DataTable();
            conn2.Close();
            return dtnull;
        }
    }


    [WebMethod]
    public DataTable DanhSach_slot_port(string dslam_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        try
        {
            string sql = "select port_id, 'slot ' || slot ||' - '|| 'port '||  port slot_port from css_hyn.port where dslam_id = '"+dslam_id+"' order by port_id";
            OracleCommand cmd = new OracleCommand(sql, conn2);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            conn2.Close();
            return dt;
        }
        catch
        {
            DataTable dtnull = new DataTable();
            conn2.Close();
            return dtnull;
        }
    }



    [WebMethod]
    public DataTable HienThi_SpliterC1C2(string port_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        try
        {
            string sql = "select * from (with lst_portvl as(    SELECT         a.portvl_id    FROM         CABMAN_HYN.PORT_GPON_SP a,        CABMAN_HYN.KETCUOI b,        ecms_HYN.PORT_GPON c,        ecms_HYN.CARD_GPON d,        css_HYN.DSLAM e,        CABMAN_HYN.view_donvi tram,        CABMAN_HYN.view_donvi tokt,        CABMAN_HYN.view_donvi ttvt    WHERE a.ketcuoi_id=b.ketcuoi_id         AND a.portvl_id=c.portvl_id        AND c.cardgp_id=d.cardgp_id        AND d.gpon_id=e.dslam_id        AND e.donvi_id=tram.donvi_id        AND tram.donvi_cha_id=tokt.donvi_id        AND tokt.donvi_cha_id=ttvt.donvi_id) select rownum stt,a.* from(    select         distinct        tbl.portvl_id,tbl.kinhdo,tbl.vido,tbl.ketcuoi_id,tbl.cap_sp,tbl.ten_kc,tbl.kyhieu,tbl.diachi,tbl.ngay_cn,        regexp_substr(tbl.info,'[^/]+', 1, 1) dungluong,        to_number(regexp_substr(tbl.info,'[^/]+', 1, 2))+ to_number(regexp_substr(tbl.info,'[^/]+', 1, 3)) dlsd,        regexp_substr(tbl.info,'[^/]+', 1, 4) dlroi,        tbl.ten_olt,tbl.system_olt    from (        SELECT            a.portvl_id, b.kinhdo,b.vido,a.ketcuoi_id,cap_sp,ten_kc,b.kyhieu,b.diachi,a.ngay_cn,            CABMAN_HYN.cabmanv2.lay_dungluong_roi_theo_ketcuoi(a.ketcuoi_id) info,            e.tendslam ten_olt,e.system system_olt,            d.vitri||'/'||c.vitri port_olt,            tram.ten_dv ten_tram,ttvt.ten_dv ten_ttvt        FROM             CABMAN_HYN.PORT_GPON_SP a,            CABMAN_HYN.KETCUOI b,            ecms_HYN.PORT_GPON c,            ecms_HYN.CARD_GPON d,            css_HYN.DSLAM e,            CABMAN_HYN.view_donvi tram,            CABMAN_HYN.view_donvi tokt,            CABMAN_HYN.view_donvi ttvt        WHERE a.ketcuoi_id=b.ketcuoi_id             AND a.portvl_id=c.portvl_id            AND c.cardgp_id=d.cardgp_id            AND d.gpon_id=e.dslam_id            AND e.donvi_id=tram.donvi_id            AND tram.donvi_cha_id=tokt.donvi_id            AND tokt.donvi_cha_id=ttvt.donvi_id            AND a.portvl_id in(select portvl_id from lst_portvl)        ORDER by c.vitri,d.vitri,b.kyhieu   ) tbl ) a ) a, css_hyn.port b where a.portvl_id = b.portvl_id and b.port_id in ("+port_id+")";
            OracleCommand cmd = new OracleCommand(sql, conn2);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            conn2.Close();
            return dt;
        }
        catch
        {
            DataTable dtnull = new DataTable();
            conn2.Close();
            return dtnull;
        }
    }


	[WebMethod]
    public string _get_usert(string taikhoan)
    {
		if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string kqua = "";
        try
        {
            string sql = "select nhanvien_id from admin_hyn.nguoidung where ma_nd='"+taikhoan+"'";
            OracleCommand cmd = new OracleCommand(sql, conn2);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            conn2.Close();
            return dt.Rows[0][0].ToString();
        }
        catch
        {
            conn2.Close();
        }
        return kqua;
    }

	[WebMethod]
		public DataTable get_datatable_khaosat(string sql)
    {
        try
        {
			if (conn2.State == ConnectionState.Closed)
			{
				conn2.Open();
			}
			OracleCommand cmd = new OracleCommand(sql, conn2);
			OracleDataAdapter da = new OracleDataAdapter(cmd);
			DataSet ds = new DataSet();
			da.Fill(ds);
			DataTable dt = new DataTable();
			if (ds.Tables.Count > 0)
			{
				dt = ds.Tables[0];
			}
			conn2.Close();
			dt.TableName = "Blah"; 
			return dt;
        }
        catch
        {
            conn2.Close();
            return null;
        }
    }
		
      

	[WebMethod]
        public string get_json_khaosat(string strQuery)
        {
            try
            {
                DataTable dt = DBProcess_Oracle.getDataTable_str("select * from tinhcuoc.khaosat", 4);
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                return serializer.Serialize(rows);


                //string json = ConvertDataTabletoJSON(dt);
                //return json;
            }
            catch
            {
                return null;
            }
        }
	[WebMethod]
        public string _dangnhap(string user, string pass)
        {
            TraCuuGachNo.TraCuuGachNo tmp = new TraCuuGachNo.TraCuuGachNo();
            string ketqua = tmp.DangNhap(user, pass);
            return ketqua;
        }
		
		
		
	[WebMethod]
    public DataTable Spliter_diaban( string vkhuvuc_id,string vcap)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.splitertheodiaban";
        objCmd.CommandType = CommandType.StoredProcedure;

		objCmd.Parameters.Add("vkhuvuc_id", OracleType.VarChar).Value = vkhuvuc_id;
		objCmd.Parameters.Add("vcap", OracleType.VarChar).Value = vcap;		
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }	
		
		
		
    [WebMethod]
    public DataTable LoadKV_ALL(string trungtam_id)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = " select * from ( select a.khuvuc_id, a.ten_kv || '-' || c.ten_nv ten_kv  from css_hyn.khuvuc a, css_hyn.khuvuc_nv b, admin_hyn.nhanvien c  where a.khuvuc_id = b.khuvuc_id and b.nhanvien_id = c.nhanvien_id  and b.loainv_id = 51 and hyn_trungtam= '"+trungtam_id+"'  UNION ALL select 0 khuvuc_id, 'Tất cả' ten_kv from dual) order by khuvuc_id";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        OracleDataAdapter da = new OracleDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }
        conn2.Close();
        return dt;
    }		
		
		
    [WebMethod]
    public DataTable dohailong_ptm_new_theotrungtam(string vtungay, string vdenngay)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.dohailong_ptm_new_theotrungtam";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }	
		
    [WebMethod]
    public DataTable dohailong_bh_new_theotrungtam(string vtungay, string vdenngay)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.dohailong_bh_new_theotrungtam";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }




    [WebMethod]
    public DataTable dohailong_ptm_new_nguyennhan(string vtungay, string vdenngay,string vtrungtam_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.dohailong_ptm_new_nguyennhan";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
        objCmd.Parameters.Add("vtrungtam_id", OracleType.VarChar).Value = vtrungtam_id;		
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }



    [WebMethod]
    public DataTable dohailong_bh_new_nguyennhan(string vtungay, string vdenngay,string vtrungtam_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.dohailong_bh_new_nguyennhan";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
		objCmd.Parameters.Add("vtrungtam_id", OracleType.VarChar).Value = vtrungtam_id;	
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }


    [WebMethod]
    public DataTable dohailong_bh_new_diaban(string vtungay, string vdenngay,string vtrungtam_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.dohailong_bh_new_diaban";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
		objCmd.Parameters.Add("vtrungtam_id", OracleType.VarChar).Value = vtrungtam_id;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }

    [WebMethod]
    public DataTable dohailong_ptm_new_diaban(string vtungay, string vdenngay,string vtrungtam_id)
    {
        OracleCommand objCmd = new OracleCommand();
        objCmd.Connection = conn2;
        objCmd.CommandText = "baocao_hyn.duy_hyn.dohailong_ptm_new_diaban";
        objCmd.CommandType = CommandType.StoredProcedure;
        //objCmd.Parameters.Add("Emp_id", OracleType.Int32).Value = vtungay; // Input id
        objCmd.Parameters.Add("vtungay", OracleType.VarChar).Value = vtungay;
        objCmd.Parameters.Add("vdenngay", OracleType.VarChar).Value = vdenngay;
		objCmd.Parameters.Add("vtrungtam_id", OracleType.VarChar).Value = vtrungtam_id;
        objCmd.Parameters.Add("returnds", OracleType.Cursor).Direction = ParameterDirection.Output;
        DataTable dt = new DataTable();
        try
        {
            conn2.Open();
            objCmd.ExecuteNonQuery();
            OracleDataAdapter da = new OracleDataAdapter(objCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: {0}", ex.ToString());
        }
        conn2.Close();
        return dt;
    }
	
	
	
	
	[WebMethod]
    public void CapNhatCongXau_BYNCMLL(string nhanvien_th_id, string ma_tb)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "update tinhcuoc.achatluong_gpon_chinh set ngay_ht = sysdate, nghiem_thu = 1, nguyennhan_id = 50, nhanvien_th_id = '"+nhanvien_th_id+"' where ma_tb='"+ma_tb+"' and nghiem_thu = 0";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        cmd.ExecuteNonQuery();
        conn2.Close();
    }
	
	
		[WebMethod]
    public void CapNhatTBKLL_BYNCMLL(string nhanvien_th_id, string ma_tb)
    {
        if (conn2.State == ConnectionState.Closed)
        {
            conn2.Open();
        }
        string sql = "update tinhcuoc.athuebao_kluuluong set ngay_ht_hyn = sysdate, nghiemthu_hyn = 1, nguyennhan_id_hyn = 50, nhanvien_th_hyn = '"+nhanvien_th_id+"' where ma_tb='"+ma_tb+"' and nguyennhan_id_hyn is null";
        OracleCommand cmd = new OracleCommand(sql, conn2);
        cmd.ExecuteNonQuery();
        conn2.Close();
    }
	
	
	
	
	
	
		
}
