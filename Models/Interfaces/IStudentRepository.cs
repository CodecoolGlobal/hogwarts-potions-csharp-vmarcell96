using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Models.Interfaces;

public interface IStudentRepository
{
    public Task<Student> GetStudentById(long id);
    public Task<List<Student>> GetAllStudents();


}