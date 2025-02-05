﻿using CliningContoraFromValera.Bll.Models;
using CliningContoraFromValera.Bll.ModelsManager;
using CliningContoraFromValera.DAL.DTOs;
using CliningContoraFromValera.DAL.ManagersInterfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using static CliningContoraFromValera.Tests.TestSources.AddressTestModels;

namespace CliningContoraFromValera.Tests
{
    public class AddressManagerTests
    {
        public AddressModelManager _addressModelManager;
        private Mock<IAddressManager> _addressManagerMock;


        [SetUp]
        public void Setup()
        {
            _addressManagerMock = new Mock<IAddressManager>();
            _addressModelManager = new AddressModelManager(_addressManagerMock.Object);
        }

        [TestCaseSource(typeof(UpdateAddressTestSource))]
        public void UpdateAddressTest(AddressModel addressModel, AddressDTO addressDto)
        {
            _addressManagerMock.Setup(o => o.UpdateAddressById(addressDto)).Verifiable();
            _addressModelManager.UpdateAddressById(addressModel);
            _addressManagerMock.Verify();
        }

        [TestCaseSource(typeof(GetAddressByIdTestSource))]
        public void GetAddressByIdAddressTest(int addressId, AddressDTO addressDto, AddressModel expectedAddress)
        {
            _addressManagerMock.Setup(o => o.GetAddressById(addressId)).Returns(addressDto).Verifiable();
            AddressModel actualAddress = _addressModelManager.GetAddressById(addressId);
            Assert.AreEqual(expectedAddress, actualAddress);
            _addressManagerMock.Verify();
        }

        [TestCaseSource(typeof(AddAddressTestSource))]
        public void AddAddressTest(AddressModel address, AddressDTO expected)
        {
            _addressManagerMock.Setup(o => o.AddAddress(expected)).Verifiable();
            _addressModelManager.AddAddress(address);
            _addressManagerMock.Verify();
        }

        [TestCaseSource(typeof(DeleteAddressByIdTestSource))]
        public void DeleteAddressByIdTest(AddressModel addressModel, AddressDTO addressDto)
        {
            _addressManagerMock.Setup(o => o.DeleteAddressById(addressDto.Id)).Verifiable();

            _addressModelManager.DeleteAddressById(addressModel.Id);

            _addressManagerMock.Verify();
        }

        [TestCaseSource(typeof(GetAllAddressTestSource))]
        public void GetAllWorkAreasTest(List<AddressDTO> addressResult, List<AddressModel> expected)
        {
            _addressManagerMock.Setup(o => o.GetAllAddresses()).Returns(addressResult).Verifiable();

            List<AddressModel> actual = _addressModelManager.GetAllAddresses();
            Assert.AreEqual(expected, actual);

            _addressManagerMock.Verify();
        }
    }
}
