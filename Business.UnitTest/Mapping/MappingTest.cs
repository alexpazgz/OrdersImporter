using AutoMapper;
using Businnes.AutoMapper;
using Domain.ApiKataEsPublico;
using Domain.Csv;
using Domain.Entities;
using Domain.Model;
using System.Runtime.Serialization;

namespace Business.UnitTest.Mapping
{
    public class MappingTest
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTest()
        {
            _configuration = new MapperConfiguration(config =>
            config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        [TestCase(typeof(OnlineOrderApiKataResponse), typeof(OnlineOrderModel))]
        [TestCase(typeof(LinkSelfApiKataResponse), typeof(LinkSelfModel))]
        [TestCase(typeof(OnlineOrderApiKataResponse), typeof(OnlineOrderModel))]
        [TestCase(typeof(OnlineOrderModel), typeof(OnlineOrder))]
        [TestCase(typeof(LinkSelfModel), typeof(Link))]
        [TestCase(typeof(OnlineOrderModel), typeof(OnlineOrderCsv))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = GetInstanceOf(source);

            _mapper.Map(instance, source, destination);
        }

        private object GetInstanceOf(Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(type)!;

            // Type without parameterless constructor
            return FormatterServices.GetUninitializedObject(type);
        }
    }
}
