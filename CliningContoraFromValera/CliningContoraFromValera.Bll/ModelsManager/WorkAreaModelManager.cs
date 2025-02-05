﻿using CliningContoraFromValera.Bll.Models;
using CliningContoraFromValera.DAL.Managers;
using CliningContoraFromValera.DAL.Managers.ManagersInterfaces;
using CliningContoraFromValera.DAL.DTOs;

namespace CliningContoraFromValera.Bll.ModelsManager
{
    public class WorkAreaModelManager
    {
        private IWorkAreaManager _workAreaManager;

        public WorkAreaModelManager()
        {
            _workAreaManager = new WorkAreaManager();
        }

        public WorkAreaModelManager(IWorkAreaManager workAreaManager)
        {
            _workAreaManager = workAreaManager;
        }

        public List<WorkAreaModel> GetAllWorkAreas()
        {
            List<WorkAreaDTO> workAreas = _workAreaManager.GetAllWorkAreas();
            return MapperConfigStorage.GetInstance().Map<List<WorkAreaModel>>(workAreas);
        }

        public WorkAreaModel GetWorkAreaById(int workAreaId)
        {
            WorkAreaDTO workArea = _workAreaManager.GetWorkAreaByID(workAreaId);
            return MapperConfigStorage.GetInstance().Map<WorkAreaModel>(workArea);
        }

        public void UpdateWorkAreaById(WorkAreaModel workAreaModel)
        {
            WorkAreaDTO workArea = MapperConfigStorage.GetInstance().Map<WorkAreaDTO>(workAreaModel);
            _workAreaManager.UpdateWorkAreaById(workArea);
        }

        public void AddWorkArea(WorkAreaModel workAreaModel)
        {
            WorkAreaDTO workArea = MapperConfigStorage.GetInstance().Map<WorkAreaDTO>(workAreaModel);
            _workAreaManager.AddWorkArea(workArea);
        }

        public void DeleteWorkAreaById(int workAreaId)
        {
            _workAreaManager.DeleteWorkAreaById(workAreaId);
        }
                
    }
}
