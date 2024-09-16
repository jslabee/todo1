﻿using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Interfaces;

public interface IOpraviloService
{
    Task<IEnumerable<Opravilo?>> GetAllOpravilaAsync();
    Task<Opravilo?> GetOpraviloByIdAsync(int id);
    Task<int> AddOpraviloAsync(Opravilo opravilo);
    Task UpdateOpraviloAsync(Opravilo opravilo);
    Task DeleteOpraviloAsync(int id);
}