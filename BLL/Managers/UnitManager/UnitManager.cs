using AutoMapper;
using BLL.DTOs;
using DAL.Data.Models;
using DAL.Repositories.CourseUnitRepo;
using DAL.Repositories.VideoRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Managers.UnitManager
{
    public class UnitManager : IUnitManager
    {
        private readonly ICourseUnitRepo _unitRepo;
        private readonly IVideoRepo _videoRepo;
        private readonly IMapper _mapper;

        public UnitManager(
            ICourseUnitRepo unitRepo,
            IVideoRepo videoRepo,
            IMapper mapper)
        {
            _unitRepo = unitRepo;
            _videoRepo = videoRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UnitDTO>> GetUnitsByCourseIdAsync(string courseId)
        {
            var units = await _unitRepo.GetUnitsByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<UnitDTO>>(units);
        }

        public async Task<UnitDTO> GetUnitByIdAsync(string id)
        {
            var unit = await _unitRepo.GetByIdAsync(id);
            if (unit == null)
                throw new KeyNotFoundException($"Unit with ID {id} not found.");

            return _mapper.Map<UnitDTO>(unit);
        }

        public async Task<string> GetCourseIdByUnitIdAsync(string unitId)
        {
            var unit = await _unitRepo.GetByIdAsync(unitId);
            if (unit == null)
                throw new KeyNotFoundException($"Unit with ID {unitId} not found.");

            return unit.CourseID;
        }


        public async Task<(bool IsUnique, string ErrorMessage)> IsUnitOrderUniqueAsync(string courseId, int order, string unitIdToExclude = null)
        {
            bool exists = await _unitRepo.OrderExistsInCourseAsync(courseId, order, unitIdToExclude);
            if (exists)
            {
                return (false, $"A unit with order {order} already exists in this course. Please choose a different order.");
            }
            return (true, null);
        }

        public async Task<(bool IsUnique, string ErrorMessage)> IsVideoOrderUniqueAsync(string unitId, int order, string videoIdToExclude = null)
        {
            bool exists = await _videoRepo.OrderExistsInUnitAsync(unitId, order, videoIdToExclude);
            if (exists)
            {
                return (false, $"A video with order {order} already exists in this unit. Please choose a different order.");
            }
            return (true, null);
        }

        public async Task<(bool Success, string Message)> AddUnitAsync(UnitDTO unitDto)
        {
            try
            {
                // Check if the order is unique in the course
                var (isUnique, errorMessage) = await IsUnitOrderUniqueAsync(unitDto.CourseID, unitDto.Order);
                if (!isUnique)
                {
                    return (false, errorMessage);
                }

                // Ensure we have a valid ID
                unitDto.Id = Guid.NewGuid().ToString();

                // Explicitly creating and mapping the entity to avoid AutoMapper issues
                var unit = new CourseUnit
                {
                    Id = unitDto.Id,
                    Title = unitDto.Title,
                    Description = unitDto.Description,
                    Order = unitDto.Order,
                    CourseID = unitDto.CourseID,
                    // Don't set the videos collection here as it will be populated later
                };

                await _unitRepo.InsertAsync(unit);
                return (true, "Unit added successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception
                return (false, $"Error adding unit: {ex.Message}");
            }
        }
        public async Task<(bool Success, string Message)> UpdateUnitAsync(UnitDTO unitDto)
        {
            try
            {
                var existingUnit = await _unitRepo.GetByIdAsync(unitDto.Id);
                if (existingUnit == null)
                    return (false, $"Unit with ID {unitDto.Id} not found.");

                // Check if the new order is unique (excluding this unit)
                if (existingUnit.Order != unitDto.Order)
                {
                    var (isUnique, errorMessage) = await IsUnitOrderUniqueAsync(unitDto.CourseID, unitDto.Order, unitDto.Id);
                    if (!isUnique)
                    {
                        return (false, errorMessage);
                    }
                }

                // Manually update properties
                existingUnit.Title = unitDto.Title;
                existingUnit.Description = unitDto.Description;
                existingUnit.Order = unitDto.Order;
                // CourseID generally shouldn't change after creation

                await _unitRepo.UpdateAsync(existingUnit);
                return (true, "Unit updated successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception
                return (false, $"Error updating unit: {ex.Message}");
            }
        }

        public async Task DeleteUnitAsync(string id)
        {
            var unit = await _unitRepo.GetByIdAsync(id);
            if (unit == null)
                throw new KeyNotFoundException($"Unit with ID {id} not found.");

            await _unitRepo.SoftDeleteAsync(unit);
        }

        public async Task<VideoDetailsDTO> GetVideoByIdAsync(string id)
        {
            var video = await _videoRepo.GetByIdAsync(id);
            if (video == null)
                throw new KeyNotFoundException($"Video with ID {id} not found.");

            return _mapper.Map<VideoDetailsDTO>(video);
        }

        public async Task<(bool Success, string Message)> AddVideoAsync(VideoDetailsDTO videoDto)
        {
            try
            {
                // Check if the order is unique in the unit
                var (isUnique, errorMessage) = await IsVideoOrderUniqueAsync(videoDto.CourseUnitID, videoDto.order);
                if (!isUnique)
                {
                    return (false, errorMessage);
                }

                // Ensure we have a valid ID
                videoDto.Id = Guid.NewGuid().ToString();

                // Explicitly create the Video entity to avoid AutoMapper issues
                var video = new Video
                {
                    Id = videoDto.Id,
                    Title = videoDto.Title,
                    Description = videoDto.Description,
                    URL = videoDto.URL,
                    order = videoDto.order,
                    CourseUnitID = videoDto.CourseUnitID,
                    ExamID = videoDto.ExamID
                    // Don't set VideoComments collection here
                };

                await _videoRepo.InsertAsync(video);
                return (true, "Video added successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception
                return (false, $"Error adding video: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateVideoAsync(VideoDetailsDTO videoDto)
        {
            try
            {
                var existingVideo = await _videoRepo.GetByIdAsync(videoDto.Id);
                if (existingVideo == null)
                    return (false, $"Video with ID {videoDto.Id} not found.");

                // Check if the new order is unique in the unit (excluding this video)
                if (existingVideo.order != videoDto.order)
                {
                    var (isUnique, errorMessage) = await IsVideoOrderUniqueAsync(videoDto.CourseUnitID, videoDto.order, videoDto.Id);
                    if (!isUnique)
                    {
                        return (false, errorMessage);
                    }
                }

                // Manually update properties
                existingVideo.Title = videoDto.Title;
                existingVideo.Description = videoDto.Description;
                existingVideo.URL = videoDto.URL;
                existingVideo.order = videoDto.order;
                // Don't update CourseUnitID as it shouldn't change
                // Don't update ExamID as it's not typically edited through this flow

                await _videoRepo.UpdateAsync(existingVideo);
                return (true, "Video updated successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception
                return (false, $"Error updating video: {ex.Message}");
            }
        }

        public async Task DeleteVideoAsync(string id)
        {
            var video = await _videoRepo.GetByIdAsync(id);
            if (video == null)
                throw new KeyNotFoundException($"Video with ID {id} not found.");

            await _videoRepo.SoftDeleteAsync(video);
        }
    }
}
