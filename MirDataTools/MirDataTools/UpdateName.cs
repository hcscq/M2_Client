using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MirDataTools.Models;
using System.IO;
namespace MirDataTools
{
    public partial class UpdateName : Form
    {
        public enum TableName {MONSTER,STDITEM,MAGIC }
        private List<string> files;
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        public UpdateName()
        {
            InitializeComponent();
        }
        private void LoadFiles(string path)
        {
            files = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories).ToList();
            dgv_files.DataSource = files;
            updateNameChanges(files);
        }

        private void btn_selDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog().Equals(DialogResult.OK))
                LoadFiles(folderBrowserDialog.SelectedPath);

        }
        private void ExportData(string fileName,int maxSelLimit=0,TableName table = TableName.MONSTER)
        {
            if (!Directory.Exists(fileName))
                Directory.CreateDirectory(fileName);
            using(MirDBContext DBcontext=new MirDBContext())
            {
                if (table.Equals(TableName.MONSTER))
                {
                    var query = from q in DBcontext.monsters
                                select q;
                    using (StreamWriter sw = new StreamWriter(File.Open(fileName, FileMode.OpenOrCreate)))
                    {
                        foreach (var item in query)
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
            }

        }
        private void updateNameChanges(List<string> files, TableName table = TableName.MONSTER)
        {
            StreamWriter sw;
            StringBuilder sb;
            FileInfo fi;
            using (MirDBContext dbContext = new MirDBContext())
            {

                List<NameModel> query=null;
                if(table.Equals(TableName.MONSTER))
                    query = (from q in dbContext.monsters
                             where q.newName != null && q.newName.Trim().Length > 0
                             select new NameModel
                             {
                                 OldName = q.列_0,
                                 NewName = q.newName
                             }).ToList();
                else if(table.Equals(TableName.STDITEM))
                    query = (from q in dbContext.stditems
                             where q.newName != null && q.newName.Trim().Length > 0
                             select new NameModel
                             {
                                 OldName = q.列_0,
                                 NewName = q.newName
                             }).ToList();
                if (query == null)
                {
                    MessageBox.Show(this,"传入表名错误或查询失败.(query==null)","错误");
                    return;
                }
                foreach (var it in files)
                {//替换文件内容.文件只读取一次
                    fi = new FileInfo(it);
                    sb = new StringBuilder(File.ReadAllText(it));
                    sw = new StreamWriter(File.Open(it, FileMode.Truncate));
                    foreach (var item in query)
                        sb.Replace(item.OldName, item.NewName);
                    sw.Write(sb.ToString());
                    sw.Flush();
                    sw.Close();
                }

                foreach (var it in query)
                {//替换文件名称.文件只读取一次
                   
                    foreach (var item in files)
                    {
                        if (item.Contains(it.OldName))
                        {
                            fi = new FileInfo(item);
                            sb = (new StringBuilder()).Append(fi.Directory).Append("\\").Append(fi.Name.Replace(it.OldName, it.NewName));
                            fi.MoveTo(sb.ToString());
                        }
                    }
                }

            }
        }
    }
}
