namespace FusionTech.src.Repository
{
    public class PersonRepository
    {
        protected DbSet<Person> _Person;
        protected DatabaseContext _databaseContext;

        public PersonRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _Person = _databaseContext.Set<Person>();
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            return await _Person.FindAsync(id);
        }

        public async Task<Person?> FindPersonByEmail(string email)
        {
            return await _Person.FirstOrDefaultAsync(e => e.PersonEmail.ToLower() == email.ToLower());
        }

        public async Task<bool> DeletePersonById(int id)
        {
            var person = await GetByIdAsync(id);
            if (person == null)
                return true;

            var result = await DeletePerson(person);
            return result;
        }

        public async Task<bool> DeletePerson(Person person)
        {
            if (person == null)
                return false;

            _Person.Remove(person);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Person person)
        {
            _Person.Update(person);
            return await _databaseContext.SaveChangesAsync() > 0;
        }

        public async Task<PersonIdCounter> GetIdCounterAsync(PersonType personType)
        {
            var counter = await _databaseContext.PersonIdCounters.SingleOrDefaultAsync(c =>
                c.PersonIdCounterId == personType
            );

            return counter!;
        }

        public async Task<bool> UpdatePersonIdCounter(PersonIdCounter idCounter)
        {
            idCounter!.CurrentId++;
            return await _databaseContext.SaveChangesAsync() > 0;
        }
        
    }
}
