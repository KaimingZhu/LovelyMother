using Microsoft.EntityFrameworkCore;
using System;

namespace Motherlibrary
{
    public class MyDatabaseContext : DbContext
    {
        public DbSet<BlackListProgress> BlackListProgresses { get; set; }
        public DbSet<Task> Tasks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DB.db");
        }
        public class Task
        {
            /// <summary>
            /// 主键。
            /// </summary>
           
            public int ID { get; set; }

            /// <summary>
            /// 创建日期。
            /// </summary>
    
            public String Date { get; set; }

            /// <summary>
            /// 开始时间。
            /// </summary>
       
            public String Begin { get; set; }

            /// <summary>
            /// 任务总时间。
            /// </summary>
            
            public int DefaultTime { get; set; }

            /// <summary>
            /// 任务当前完成时间。
            /// </summary>
         
            public int FinishTime { get; set; }

            /// <summary>
            /// 任务说明。
            /// </summary>
         
            public String Introduction { get; set; }

            /// <summary>
            /// 是否完成任务。
            /// </summary>

            public int Finish { get; set; }

            /// <summary>
            /// 所属用户ID。
            /// </summary>


        }
        
       public class BlackListProgress
        {
            /// <summary>
            /// 主键。
            /// </summary>
     
            public int ID { get; set; }

            /// <summary>
            /// 进程名称。
            /// </summary>
           
            public string FileName { get; set; }

            /// <summary>
            /// 用户定义名称。
            /// </summary>
          
            public string ResetName { get; set; }

            /// <summary>
            /// 种类。
            /// </summary>

            public int Type { get; set; }


        }
        
        }
    }

