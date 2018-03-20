using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Newtonsoft.Json;
using FluentAssertions;

namespace ACME.MicrosoftCA.Gateway.Unit.Tests
{
    public class ModelsTest
    {
        
        public class SubProblem1 : Models.API.IdentifiedProblem
        {
            public SubProblem1 ()
            {
                ProblemType = Models.API.ProblemType.malformed;
                Detail = @"Sub problem 1";
            }

        }

        [Fact]
        public void API_Problem_Serialization()
        {
            var correct = "{\"type\":\"urn:ietf:params:acme:error:none\",\"detail\":null}";
            var problem = new Models.API.Problem();
            
            var s = JsonConvert.SerializeObject(problem);

            s.Should().BeEquivalentTo(correct);
        }

        [Fact]
        public void API_SubProblem_Serialization()
        {
            var correct = "{\"subproblems\":[{\"type\":\"urn:ietf:params:acme:error:none\",\"detail\":null}],\"type\":\"urn:ietf:params:acme:error:none\",\"detail\":null}";
            var problem = new Models.API.ProblemWithSubProblems();

            problem.AddSubProblem(new Models.API.Problem());
            var s = JsonConvert.SerializeObject(problem);

            s.Should().BeEquivalentTo(correct);

        }
        [Fact]
        public void API_SubProblem_Identified_Serialization()
        {
            var correct = "{\"subproblems\":[{\"identifier\":null,\"type\":\"urn:ietf:params:acme:error:malformed\",\"detail\":\"Sub problem 1\"}],\"type\":\"urn:ietf:params:acme:error:none\",\"detail\":null}";
            var problem = new Models.API.ProblemWithSubProblems();

            problem.AddSubProblem(new SubProblem1());
            var s = JsonConvert.SerializeObject(problem);

            s.Should().BeEquivalentTo(correct);
        }
    }
}
