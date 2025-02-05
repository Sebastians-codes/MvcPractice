﻿using Bulky.Domain.Models;

namespace Bulky.Data.Repository.IRepository;

public interface ICategoryRepository : IRepository<Category>
{
    void Update(Category category);
}