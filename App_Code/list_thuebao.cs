using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for list_thuebao
/// </summary>
public class list_thuebao
{
    public List<thuebao> listtb { get; set; }
    public list_thuebao(List<thuebao> _listtb)
    {
        this.listtb = _listtb;
    }
    public list_thuebao()
        {

        }
}