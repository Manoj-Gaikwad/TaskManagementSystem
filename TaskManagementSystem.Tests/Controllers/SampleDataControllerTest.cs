using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Repository;
using Xunit;

namespace TaskManagementSystem.Tests.Controllers
{
    public class SampleDataControllerTest
    {

        [Fact]
        public void HomeController_HelloPrint_ValidResult()
        {
            SampleDataController controller = new SampleDataController();
            string ExpectedResult = "Hello From Test";
            string result = controller.HelloPrint();
            Assert.Equal(ExpectedResult, result);
        }
    }
}
