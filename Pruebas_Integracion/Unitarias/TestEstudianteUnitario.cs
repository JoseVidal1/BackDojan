using BLL;
using DAL;
using DAL.Modelos;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas.Unitarias
{
    public  class TestEstudianteUnitario
    {
        [Fact]
        public async Task Agregar_ReturnsEstudiante_WhenSuccess()
        {
            // Arrange
            var mockDBEst = new Mock<IDBEstudiante>();
            var mockDBGrupo = new Mock<IDBGrupo>();

            var estudiante = new Estudiante
            {
                Id = "123",
                Nombres = "Juan",
                Apellidos = "Pérez",
                Telefono = "3001234567",
                Correo = "juan@test.com",
                Direccion = "Calle 1",
                Eps = "Sanitas",
                IdRango = 1,
                FechaNacimiento = new DateOnly(2015, 1, 1) // Edad = 10
            };

            // 1. Primero el estudiante NO existe
            mockDBEst.SetupSequence(x => x.Buscar("123"))
                .ReturnsAsync((Estudiante?)null)    // primera llamada
                .ReturnsAsync(estudiante);          // segunda llamada

            // 2. Simular el agregado
            mockDBEst.Setup(x => x.Agregar(estudiante))
                     .Returns(Task.CompletedTask);

            var service = new ServiceEstudiante(mockDBEst.Object, mockDBGrupo.Object);

            // Act
            var result = await service.Agregar(estudiante);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("123", result.Id);
            Assert.Equal("Activo", result.estado);
            Assert.Equal(1, result.IdGrupo); // Edad 10 → Grupo 1
            Assert.Equal(10, result.edad);

            mockDBEst.Verify(x => x.Agregar(estudiante), Times.Once);
        }

    }
}
