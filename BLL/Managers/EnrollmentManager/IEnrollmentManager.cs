using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Managers.EnrollmentManager
{
    public interface IEnrollmentManager
    {
        Task<bool> EnrollAccountAsync(string AccountId, int courseId);
    }
}

