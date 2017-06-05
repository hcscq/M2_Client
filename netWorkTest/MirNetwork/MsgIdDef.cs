using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netWorkTest.MirNetwork
{
    class MsgIdDef
    {
        public const int CM_PROTOCOL = 2000;
        public const int CM_IDPASSWORD = 2001;
        public const int CM_ADDNEWUSER = 2002;
        public const int CM_CHANGEPASSWORD = 2003;
        public const int CM_UPDATEUSER = 2004;

        public const int CM_SELECTSERVER = 104;

        public const int SM_CERTIFICATION_FAIL = 501;
        public const int SM_ID_NOTFOUND = 502;
        public const int SM_PASSWD_FAIL = 503;
        public const int SM_NEWID_SUCCESS = 504;
        public const int SM_NEWID_FAIL = 505;
        public const int SM_PASSOK_SELECTSERVER = 529;
        public const int SM_SELECTSERVER_OK = 530;
    }
}
