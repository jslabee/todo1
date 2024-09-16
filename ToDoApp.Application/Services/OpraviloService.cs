using log4net;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.Application.Services;

public class OpraviloService(IOpraviloRepository opraviloRepository) : IOpraviloService
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(OpraviloService));

    public async Task<IEnumerable<Opravilo?>> GetAllOpravilaAsync()
    {
        try
        {
            return await opraviloRepository.GetAllOpravilaAsync();
        }
        catch (Exception ex)
        {
            Logger.Error("Prišlo je do napake pri pridobivanju vseh opravil.", ex);
            throw new ApplicationException("Prišlo je do napake pri pridobivanju vseh opravil.", ex);
        }
    }

    public async Task<Opravilo?> GetOpraviloByIdAsync(int id)
    {
        try
        {
            var opravilo = await opraviloRepository.GetOpraviloByIdAsync(id);

            if (opravilo == null)
            {
                Logger.Warn($"Opravilo z ID-jem {id} ni bilo najdeno.");
                throw new KeyNotFoundException($"Opravilo z ID-jem {id} ni bilo najdeno.");
            }

            return opravilo;
        }
        catch (KeyNotFoundException ex)
        {
            Logger.Warn($"Opravilo z ID-jem {id} ni bilo najdeno.", ex);
            throw;
        }
        catch (Exception ex)
        {
            Logger.Error($"Prišlo je do napake pri pridobivanju opravil z ID-jem {id}.", ex);
            throw new ApplicationException($"Prišlo je do napake pri pridobivanju opravil z ID-jem {id}.", ex);
        }
    }


    public async Task<int> AddOpraviloAsync(Opravilo opravilo)
    {
        try
        {
            // Preveri, ali naslov ni prazen
            if (string.IsNullOrEmpty(opravilo.Naslov))
            {
                Logger.Warn("Preverjanje ni uspelo: Naslov opravil ne sme biti prazen.");
                throw new ArgumentException("Naslov opravil ne sme biti prazen.");
            }

            // Preveri, ali opis ni prazen
            if (string.IsNullOrEmpty(opravilo.Opis))
            {
                Logger.Warn("Preverjanje ni uspelo: Opis opravil ne sme biti prazen.");
                throw new ArgumentException("Opis opravil ne sme biti prazen.");
            }

            // Dodaj opravilo v bazo
            return await opraviloRepository.AddOpraviloAsync(opravilo);
        }
        catch (Exception ex)
        {
            Logger.Error("Prišlo je do napake pri dodajanju novega opravila.", ex);
            throw new ApplicationException("Prišlo je do napake pri dodajanju novega opravil.", ex);
        }
    }

    public async Task UpdateOpraviloAsync(Opravilo opravilo)
    {
        try
        {
            await opraviloRepository.UpdateOpraviloAsync(opravilo);
        }
        catch (Exception ex)
        {
            Logger.Error($"Prišlo je do napake pri posodabljanju opravil z ID-jem {opravilo.Id}.", ex);
            throw new ApplicationException($"Prišlo je do napake pri posodabljanju opravil z ID-jem {opravilo.Id}.", ex);
        }
    }

    public async Task DeleteOpraviloAsync(int id)
    {
        try
        {
            await opraviloRepository.DeleteOpraviloAsync(id);
        }
        catch (KeyNotFoundException ex)
        {
            Logger.Error($"Opravilo z ID-jem {id} ni bilo najdeno za izbris.", ex);
            throw;
        }
        catch (Exception ex)
        {
            Logger.Error($"Prišlo je do napake pri brisanju opravila z ID-jem {id}.", ex);
            throw new ApplicationException($"Prišlo je do napake pri brisanju opravila z ID-jem {id}.", ex);
        }
    }
}
