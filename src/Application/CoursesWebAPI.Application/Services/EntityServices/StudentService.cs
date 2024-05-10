using CoursesWebAPI.Application.Contracts.Data;
using CoursesWebAPI.Application.Contracts.EntityServices;
using CoursesWebAPI.Application.Mappers;
using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Core.Contracts.Repositories;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using CoursesWebAPI.Shared.RequestFeatures;
using Serilog;
using System.Runtime.InteropServices;

namespace CoursesWebAPI.Application.Services.EntityServices
{
    public sealed class StudentService : IStudentService
    {
        private readonly ILogger logger;
        private readonly IStudentRepository studentRepository;
        private readonly IRepositoryManager repositoryManager;

        public StudentService(ILogger logger, IRepositoryManager repositoryManager)
        {
            this.logger = logger;
            this.repositoryManager = repositoryManager;
            studentRepository = repositoryManager.StudentRepository;
        }

        public async Task<StudentDto> CreateAsync(StudentForCreatingDto studentDto, CancellationToken cancellationToken = default)
        {
            var newStudent = studentDto.ToStudent();
            studentRepository.Create(newStudent);
            await repositoryManager.SaveChangesAsync(cancellationToken);
            logger.Information("New student: {Firstname} {Lastname} {Patronomic} was created.",
                newStudent.FirsName, newStudent.LastName, newStudent.Patronomic);
            return newStudent.ToStudentDto();
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            studentRepository.Delete(id);
            await repositoryManager.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<StudentForUpdateDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var student = await studentRepository.GetByIdAsync(id, false, cancellationToken);
            return student?.ToStudentForUpdateDto();
        }

        public async Task<IEnumerable<StudentForUpdateDto>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            var students = await studentRepository.GetByExpressionAsync(s => ids.Contains(s.Id), false, cancellationToken);
            return new List<StudentForUpdateDto>(students.Select(s => s.ToStudentForUpdateDto()));
        }

        public async Task<StudentDto?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var result = await studentRepository.GetByExpressionAsync(s => s.UserId.Equals(userId), trackChanges: false, cancellationToken);
            if (!result.Any())
            {
                return null;
            }
            return result.First().ToStudentDto();
        }

        public async Task<PageList<StudentForUpdateDto>> GetStudentsAsync(PersonQueryParameters queryParameters, CancellationToken cancellationToken = default)
        {
            var studentsPageList = await studentRepository.GetByParametersAsync(queryParameters, cancellationToken);
            var studentDtos = studentsPageList.Select(s => s.ToStudentForUpdateDto());
            return PageList<StudentForUpdateDto>.ToPageList(studentDtos, studentsPageList.MetaData);
        }

        public async Task<StudentDto> UpdateAsync(StudentForUpdateDto studentDto, CancellationToken cancellationToken = default)
        {
            var studentForUpdate = studentDto.ToStudent();
            studentRepository.Update(studentForUpdate);
            await repositoryManager.SaveChangesAsync(cancellationToken);
            return studentForUpdate.ToStudentDto();
        }
    }
}
