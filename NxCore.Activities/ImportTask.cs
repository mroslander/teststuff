using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxCore.Activities
{
    public class ImportTask
    {
        public List<string> FilesToImport { get; set; }
        public string ImportSetName { get; set; }

        public static ImportTask CreateTestTask(int fileCount = 180)
        {
            List<string> filesToImport = new List<string>(fileCount);
            for (int i = 1; i <= fileCount; i++)
            {
                filesToImport.Add(string.Format("File {0}", i));
            }

            return new ImportTask()
            {
                FilesToImport = 
                filesToImport,
                ImportSetName = "nupas"
            };
        }
    }
}
