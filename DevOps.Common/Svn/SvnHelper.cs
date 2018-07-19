using SharpSvn;
using SharpSvn.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevOps.Common.Svn
{
    public class SvnHelper
    {

        /// <summary>
        /// 设置用户名密码
        /// </summary>
        SvnHelper(string userName, string password)
        {
            using (SvnClient client = new SvnClient())
            {
                client.Authentication.Clear(); //清除原有的账户信息
                client.Authentication.UserNamePasswordHandlers += new EventHandler<SvnUserNamePasswordEventArgs>(
                        delegate (object s, SvnUserNamePasswordEventArgs e)
                        {
                            e.UserName = userName;
                            e.Password = password;
                        });
            }
        }

        /// <summary>
        /// 证书确认
        /// </summary>
        private void tt()
        {
            using (SvnClient client = new SvnClient())
            {
                client.Authentication.SslServerTrustHandlers += new EventHandler<SvnSslServerTrustEventArgs>(
                    delegate (object ssender, SvnSslServerTrustEventArgs e)
                {
                    e.AcceptedFailures = e.Failures;
                    e.Save = true; // Save acceptance to authentication store
                    //e.Cancel = true; //表示不信任该服务器,放弃访问
                });

            }

        }

        /// <summary>
        /// 取消操作
        /// </summary>
        private void Cancel()
        {
            using (SvnClient client = new SvnClient())
            {
                // do something
                client.Cancel +=
                    delegate (object s, SvnCancelEventArgs e)
                    {
                        e.Cancel = true;
                    };
                // do svn operations
            }
        }
    }
}
