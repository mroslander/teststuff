using Microsoft.VisualStudio.TestTools.UnitTesting;
using NxCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxCore.Data.Tests
{
    [TestClass()]
    public class ProfileCuttingContextTests
    {
        [TestMethod()]
        public void ProfileCuttingContextTest()
        {
            using (var ctx = new ProfileCuttingContext())
            {
                for (int batchNo = 0; batchNo < 10; batchNo++)
                {
                    BatchData batch = new BatchData() { Title = "Batch" + batchNo };
                    batch.JobDatas = new List<JobData>();

                    for (int jobNo = 0; jobNo < 20; jobNo++)
                    {
                        JobData job = new JobData() { Title = "Job" + jobNo };
                        job.BatchData = batch;
                        job.ProfileMaterialDatas = new List<ProfileMaterialData>();
                        job.ProfilePartDatas = new List<ProfilePartData>();
                        //ctx.Jobs.Add(job);
                        batch.JobDatas.Add(job);

                        for (int materialNo = 0; materialNo < 3; materialNo++)
                        {
                            ProfileMaterialData materialData = new ProfileMaterialData()
                            {
                                Title = "Material" + materialNo,
                                Length = 6000,
                                Width = 500
                            };
                            materialData.JobData = job;
                            //ctx.ProfileMaterials.Add(materialData);
                            job.ProfileMaterialDatas.Add(materialData);
                        }
                        for (int profilePartNo = 0; profilePartNo < 20; profilePartNo++)
                        {
                            ProfilePartData profilePartData = new ProfilePartData()
                            {
                                Title = "ProfilePart" + profilePartNo,
                                Length = 1000,
                                Width = 500
                            };
                            profilePartData.JobData = job;
                            //ctx.ProfileParts.Add(profilePartData);
                            job.ProfilePartDatas.Add(profilePartData);
                        }
                    }

                    ctx.Batches.Add(batch);
                }

                ctx.SaveChanges();
            }

        }

        [TestMethod()]
        public void ProfileCuttingContextTest2()
        {
            using (var ctx = new ProfileCuttingContext())
            {
                foreach (JobData job in ctx.Jobs)
                {
                    Console.WriteLine("Job " + job.Title);
                }

                foreach (BatchData job in ctx.Batches)
                {
                }

                    foreach (JobData job in ctx.Jobs)
                {
                    Console.WriteLine("Job " + job.Title);
                }
            }
        }
    }
}