using System;
using System.Collections;

namespace LearnApp.Models{
    public interface ICourseRepository{
        public void AddCourse(Course course);
        public void DeleteCourse(string id);
        public List<Course> GetCourses();
        public IEnumerable FetchCoursesForBatch(string batchid);

        public Course FetchDetailsForPlaylist(string batchid,string coursename);
        public bool CheckCourseOnBatchId(string coursename,string batchid);
    }
}