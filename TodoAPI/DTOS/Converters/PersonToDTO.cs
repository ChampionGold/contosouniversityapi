using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Models;
using TodoAPI.DTOS;

namespace TodoAPI.DTOS.Converters
{
    public static class PersonToDTO
    {
        public static Person StudentToDTO(Student student)
        {
            return new Person {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName
            };

        }

    }
}
