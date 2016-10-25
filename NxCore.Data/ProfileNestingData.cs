using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxCore.Data
{    
    public class ProfilePartData
    {
        public int ProfilePartDataId { get; set; }        

        public string Title { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }

        public double FreeCount { get; set; }

        public int JobDataId { get; set; }
        public JobData JobData { get; set; }        
    }

    public class ProfileMaterialData
    {
        public int ProfileMaterialDataId { get; set; }

        public string Title { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }

        public double FreeCount { get; set; }

        public int JobDataId { get; set; }
        public JobData JobData { get; set; }
    }

    public class ProfileNestingData
    {
        public int ProfileNestingDataId { get; set; }
        public string Title { get; set; }

        public int JobDataId { get; set; }
        public JobData JobData { get; set; }
    }

    public class JobData
    {
        public int JobDataId { get; set; }       

        public string Title { get; set; }

        public IList<ProfilePartData> ProfilePartDatas { get; set; }
        public IList<ProfileMaterialData> ProfileMaterialDatas { get; set; }
        public IList<ProfileNestingData> ProfileNestingDatas { get; set; }

        public int BatchDataId { get; set; }
        public BatchData BatchData { get; set; }

        public override string ToString()
        {
            return string.Format("{0} with {1} parts, {2} materials, {3} nestings", Title, ProfilePartDatas.Count, ProfileMaterialDatas.Count, ProfileNestingDatas.Count);
        }
    }

    public class BatchData
    {
        public int BatchDataId { get; set; }
        public string Title { get; set; }

        public ICollection<JobData> JobDatas { get; set; }

        public override string ToString()
        {
            return string.Format("{0} with {1} jobs", Title, JobDatas.Count);
        }
    }

    public class ProfileCuttingData
    {
        public string UserName { get; set; }

        public IList<BatchData> BatchDatas { get; set; }

        public static ProfileCuttingData CreateTestData()
        {
            ProfileCuttingData data = new ProfileCuttingData();
            data.UserName = System.Environment.UserName;
            data.BatchDatas = new List<BatchData>();

            int batchesPerData = 1;
            int jobsPerBatch = 3;
            int materialsPerJob = 1;
            int partsPerJob = 5;

            //using (var ctx = new ProfileCuttingContext())
            //{
            for (int batchNo = 0; batchNo < batchesPerData; batchNo++)
            {
                BatchData batch = new BatchData() { Title = "Batch" + batchNo };
                batch.JobDatas = new List<JobData>();

                for (int jobNo = 0; jobNo < jobsPerBatch; jobNo++)
                {
                    JobData job = new JobData() { Title = "Job" + jobNo };
                    //job.BatchData = batch;
                    job.ProfileMaterialDatas = new List<ProfileMaterialData>();
                    job.ProfilePartDatas = new List<ProfilePartData>();
                    job.ProfileNestingDatas = new List<ProfileNestingData>();

                    batch.JobDatas.Add(job);

                    for (int materialNo = 0; materialNo < materialsPerJob; materialNo++)
                    {
                        ProfileMaterialData materialData = new ProfileMaterialData()
                        {
                            Title = "Material" + materialNo,
                            Length = 6000,
                            Width = 500
                        };
                        //materialData.JobData = job;                            
                        job.ProfileMaterialDatas.Add(materialData);
                    }
                    for (int profilePartNo = 0; profilePartNo < partsPerJob; profilePartNo++)
                    {
                        ProfilePartData profilePartData = new ProfilePartData()
                        {
                            Title = "ProfilePart" + profilePartNo,
                            Length = 1000,
                            Width = 500
                        };
                        //profilePartData.JobData = job;                            
                        job.ProfilePartDatas.Add(profilePartData);
                    }
                }

                data.BatchDatas.Add(batch);
            }

            //ctx.SaveChanges();
            //}

            return data;
        }
    }

    // ----

    public class ProfileCuttingContext : DbContext
    {
        public ProfileCuttingContext() : base()
        {

        }

        public DbSet<BatchData> Batches { get; set; }
        public DbSet<JobData> Jobs { get; set; }
        public DbSet<ProfilePartData> ProfileParts { get; set; }
        public DbSet<ProfileMaterialData> ProfileMaterials { get; set; }
        public DbSet<ProfileNestingData> ProfileNestings { get; set; }

    }

    
   

}
