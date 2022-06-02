﻿using CliningContoraFromValera.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliningContoraFromValera.DAL;
using CliningContoraFromValera.DAL.Managers;
using CliningContoraFromValera.DAL.DTOs;


namespace CliningContoraFromValera.Bll.ModelsManager
{
    public class ClientModelManager
    {
        ClientManager ClientManager = new ClientManager();

        public List<ClientModel> GetAll()
        {
            List<ClientDTO> clients = ClientManager.GetAllClients();
            return MapperConfigStorage.GetInstance().Map<List<ClientModel>>(clients);
        }

        public ClientModel GetClientById(int clientId)
        {
            ClientDTO client = ClientManager.GetClientByID(clientId);
            return MapperConfigStorage.GetInstance().Map<ClientModel>(client);
        }

        public void UpdateClientByID(ClientModel clientModel)
        {
            ClientDTO client = MapperConfigStorage.GetInstance().Map<ClientDTO>(clientModel);
            ClientManager.UpdateClientById(client);
        }

        public void AddClient(ClientModel clientModel)
        {
            ClientDTO client = MapperConfigStorage.GetInstance().Map<ClientDTO>(clientModel);
            ClientManager.AddClient(client);
        }

        public void Delete(int id)
        {
            ClientDTO client = MapperConfigStorage.GetInstance().Map<ClientDTO>(id);
            ClientManager.DeleteClientById(client.Id);
        }

    }
}