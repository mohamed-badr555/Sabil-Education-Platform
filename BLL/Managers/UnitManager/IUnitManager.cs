using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Managers.UnitManager
{
    public interface IUnitManager
    {
        Task<IEnumerable<UnitDTO>> GetUnitsByCourseIdAsync(string courseId);
        Task<UnitDTO> GetUnitByIdAsync(string id);
        Task<string> GetCourseIdByUnitIdAsync(string unitId);
        Task<(bool Success, string Message)> AddUnitAsync(UnitDTO unitDto);
        Task<(bool Success, string Message)> UpdateUnitAsync(UnitDTO unitDto);
        Task DeleteUnitAsync(string id);
        Task<VideoDetailsDTO> GetVideoByIdAsync(string id);
        Task<(bool Success, string Message)> AddVideoAsync(VideoDetailsDTO videoDto);
        Task<(bool Success, string Message)> UpdateVideoAsync(VideoDetailsDTO videoDto);
        Task DeleteVideoAsync(string id);
        Task<(bool IsUnique, string ErrorMessage)> IsUnitOrderUniqueAsync(string courseId, int order, string unitIdToExclude = null);
        Task<(bool IsUnique, string ErrorMessage)> IsVideoOrderUniqueAsync(string unitId, int order, string videoIdToExclude = null);
    }
}
