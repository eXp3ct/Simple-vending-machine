using AutoMapper;
using Expect.Vending.Data.Interfaces;
using Expect.Vending.Domain.Dtos;
using Expect.Vending.Domain.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Expect.Vending.Web.Controllers
{
    /// <summary>
    /// Контроллер напитков
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class DrinkController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public DrinkController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Создание напитка в бд
        /// </summary>
        /// <param name="dto">Информация о напитке</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Созданные напиток</returns>
        [HttpPost]
        public async Task<IActionResult> CreateDrink([FromBody] DrinkDto dto, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.Repository<Drink>();

            var drink = _mapper.Map<Drink>(dto);

            var created = await repository.Add(drink, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Ok(created);
        }

        /// <summary>
        /// Получение всех напитков
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Список всех напитков</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Drink>();
            var drinks = await repo.GetAll(cancellationToken);

            return Ok(drinks);
        }

        /// <summary>
        /// Удаление напитка из бд
        /// </summary>
        /// <param name="id">Id напитка</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Удаленный напиток</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteDrink([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Drink>();
            var deleted = await repo.Delete(id, cancellationToken);

            if (deleted is null)
                return NotFound();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(deleted);
        }

        /// <summary>
        /// Обновление напитка в бд
        /// </summary>
        /// <param name="id">Id напитка</param>
        /// <param name="dto">Новая информация о напитке</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Обновленный напиток</returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateDrink([FromRoute] Guid id, [FromBody]  DrinkDto dto, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Drink>();
            var newDrink = _mapper.Map<Drink>(dto);

            var updated = await repo.Update(id, newDrink, cancellationToken);
            if(updated is null)
                return NotFound();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(updated);
        }

        /// <summary>
        /// Покупка напитка
        /// </summary>
        /// <param name="id">Id напитка</param>
        /// <param name="quantity">Количество</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Обновленный напиток</returns>
        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> BuyDrink([FromRoute] Guid id, int quantity, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Drink>();
            var drink = await repo.GetById(id, cancellationToken);
            if (drink is null) return NotFound();

            drink.Quantity -= quantity;

            var updated = await repo.Update(id, drink, cancellationToken);

            if (updated is null) return NotFound();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(drink);
        }

        /// <summary>
        /// Привязка картинки к напитку
        /// </summary>
        /// <param name="id">Id напитка</param>
        /// <param name="imageId">Id картинки</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Напиток</returns>
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> LinkImage([FromRoute] Guid id, Guid imageId, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Drink>();
            var drink = await repo.GetById(id, cancellationToken);
            if (drink is null) return NotFound();

            drink.ImageId = imageId;

            var updated = await repo.Update(id, drink, cancellationToken);

            if (updated is null) return NotFound();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Ok(updated);
        }
    }
}
