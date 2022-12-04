using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Services.Interfaces;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;
using System.Net;

namespace FreeCourse.Services.Catalog.Services
{
    internal class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(c => true).ToListAsync();

            if (courses.Any())
            {
                var getCategoryTasks = courses.Select(async (course) =>
                {
                    course.Category = await _categoryCollection.Find(c => c.Id.Equals(course.CategoryId)).FirstOrDefaultAsync();
                }).ToArray();
                Task.WaitAll(getCategoryTasks);
            }

            return Response<IEnumerable<CourseDto>>.Success(_mapper.Map<IEnumerable<CourseDto>>(courses), HttpStatusCode.OK);
        }

        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find<Course>(c => c.Id.Equals(id)).SingleOrDefaultAsync();

            if (course == null)
                return Response<CourseDto>.Fail("Course not found", HttpStatusCode.NotFound);

            course.Category = await _categoryCollection.Find(c => c.Id.Equals(course.CategoryId)).SingleOrDefaultAsync();

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), HttpStatusCode.OK);
        }

        public async Task<Response<IEnumerable<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find(c => c.UserId.Equals(userId)).ToListAsync();

            if (courses.Any())
            {
                var getCategoryTasks = courses.Select(async (course) =>
                {
                    course.Category = await _categoryCollection.Find(c => c.Id.Equals(course.CategoryId)).FirstOrDefaultAsync();
                }).ToArray();
                Task.WaitAll(getCategoryTasks);
            }



            return Response<IEnumerable<CourseDto>>.Success(_mapper.Map<IEnumerable<CourseDto>>(courses), HttpStatusCode.OK);
        }

        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreate)
        {
            var newCourse = _mapper.Map<Course>(courseCreate);
            newCourse.CreatedTime = DateTime.Now;
            await _courseCollection.InsertOneAsync(newCourse);

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), HttpStatusCode.OK);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdate)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdate);

            var result = await _courseCollection.FindOneAndReplaceAsync(c => c.Id.Equals(courseUpdate.Id), updateCourse);

            return result != null ?
                Response<NoContent>.Success(HttpStatusCode.NoContent) :
                Response<NoContent>.Fail("Course not found", HttpStatusCode.NotFound);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(c => c.Id.Equals(id));

            return result.DeletedCount > 0 ?
                Response<NoContent>.Success(HttpStatusCode.NoContent) :
                Response<NoContent>.Fail("Course not found", HttpStatusCode.NotFound);
        }
    }
}