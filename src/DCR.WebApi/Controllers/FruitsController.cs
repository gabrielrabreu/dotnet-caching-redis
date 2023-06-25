using DCR.WebApi.Contracts;
using DCR.WebApi.Extensions;
using DCR.WebApi.Models;
using DCR.WebApi.Repositories;
using DCR.WebApi.Scope.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;

namespace DCR.WebApi.Controllers
{
    [ApiController]
    [Route("api/fruits")]
    public class FruitsController : ControllerBase
    {
        private const string CacheKey = "FruitsKey";

        private readonly ILogger<FruitsController> _logger;
        private readonly IDistributedCache _cache;
        private readonly IFruitRepository _repository;

        public FruitsController(ILogger<FruitsController> logger, IDistributedCache cache, IFruitRepository repository)
        {
            _logger = logger;
            _cache = cache;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Request.Headers.TryGetValue(CustomHeaderSwaggerAttribute.UseCacheHeader, out StringValues header);
            var shouldUseCache = header.Any();

            List<FruitModel>? fruits;

            if (shouldUseCache)
            {
                FetchFromCacheOrCreate(out fruits);
            }
            else
            {
                fruits = _repository.GetAll();
            }

            return Ok(fruits?.Select(ToDto));
        }

        [HttpGet("{id:guid}")]
        public IActionResult Details(Guid id)
        {
            Request.Headers.TryGetValue(CustomHeaderSwaggerAttribute.UseCacheHeader, out StringValues header);
            var shouldUseCache = header.Any();

            FruitModel? fruit;

            if (shouldUseCache)
            {
                FetchFromCacheOrCreate(out var fruits);
                fruit = fruits?.SingleOrDefault(x => x.Id == id);
            }
            else
            {
                fruit = _repository.GetById(id);
            }

            if (fruit == null) return NoContent();

            return Ok(ToDto(fruit));
        }

        [HttpPost]
        public IActionResult Create([FromBody] FruitDto dto)
        {
            var model = new FruitModel(dto.Name);

            _repository.Add(model);
            _cache.Remove(CacheKey);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public IActionResult Edit(Guid id, [FromBody] FruitDto dto)
        {
            var model = _repository.GetById(id);

            if (model == null) return BadRequest();

            model.Name = dto.Name;

            _repository.Update(model);
            _cache.Remove(CacheKey);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var model = _repository.GetById(id);

            if (model == null) return NoContent();

            _repository.Delete(model);
            _cache.Remove(CacheKey);

            return NoContent();
        }

        private FruitForViewDto ToDto(FruitModel model)
        {
            return new FruitForViewDto
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        private void FetchFromCacheOrCreate(out List<FruitModel>? fruits)
        {
            _logger.LogInformation("Trying to fetch the fruits from redis cache.");

            if (_cache.TryGetValue(CacheKey, out fruits))
            {
                _logger.LogInformation("Fruits found in redis cache.");
            }
            else
            {
                _logger.LogInformation("Fruits not found in redis cache. Fetching from database.");

                fruits = _repository.GetAll();

                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(600))
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60));

                _cache.Set(CacheKey, fruits, cacheEntryOptions);
            }
        }
    }
}
