using System;
using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.Services
{
    /// <summary>
    /// Represents a set of methods for user management.
    /// </summary>
    public interface IUserManagementService
    {
        /// <summary>
        /// Gets user by id.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <returns>
        /// Returns the user associated with the given id as an instance of the class <see cref="UserForDisplay"/>.
        /// </returns>
        UserForDisplay GetUserById(Guid id);

        /// <summary>
        ///  Updates the information of a user assosiated with the given id..
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <param name="userForUpdate"></param>
        void UpdateUser(Guid id, UserForUpdate userForUpdate);

        /// <summary>
        /// Adds a new dog.
        /// </summary>
        /// <param name="dogToBeAdded">The dog that is going to be added.</param>
        /// <param name="ownerId">Th id of the owner.</param>
        void AddDog(DogToBeAdded dogToBeAdded, Guid ownerId);

        /// <summary>
        /// Updates dog information associated with the given id.
        /// </summary>
        /// <param name="dogId">Id of the dog.</param>
        /// <param name="dogForUpdate">An instance of the <see cref="DogForUpdate"/> class that keeps the new data.</param>
        void UpdateDog(Guid dogId, DogForUpdate dogForUpdate);

        /// <summary>
        /// Gets dog by id.
        /// </summary>
        /// <param name="id">Id of the dog.</param>
        /// <returns>The dog associated with the given id as an instance of the class <see cref="DogFromDB"/>.</returns>
        DogFromDB GetDogById(Guid id);


        /// <summary>
        /// Gets the next dogs from database for the given page.
        /// </summary>
        /// <param name="page">Page number.</param>
        ///<param name="currentOwnerId">Id of current owner.</param>
        /// <returns>Returns a list of dogs as an instance of the class <see cref="DogForDisplay"/>.</returns>
        List<DogForDisplayWithId> GetAllDogs(int page, Guid currentOwnerId);

        /// <summary>
        /// Gets filtered dogs from database for the given page.
        /// </summary>
        /// <param name="page">Page number.</param>
        ///<param name="filters">>An instance of the <see cref="Filters".</param>
        ///<param name="currentOwnerId">Id of current owner.</param>
        /// <returns>Returns a list of filtered dogs as an instance of the class <see cref="DogForDisplay"/>.</returns>
        List<DogForDisplayWithId> FilterDogs(int page, Filters filters, Guid currentOwnerId);

        /// <summary>
        /// Gets dogs of the given ownner.
        /// </summary>
        ///<param name="ownersId">Id of owner.</param>
        /// <returns>Returns a list of dogs as an instance of the class <see cref="DogForDisplay"/>.</returns>
        List<DogForDisplayWithId> GetOwnersDogs(Guid ownersId);

        /// <summary>
        /// Finds the total number of dogs in the system.
        /// </summary>
        ///<param name="currentOwnerId">Id of current owner.</param>
        /// <returns>The number of dogs in the system.</returns>
        int GetDogEntriesCount(Guid currentOwnerId);

        /// <summary>
        ///  Finds the total number of dogs in the system returned by a filtering function.
        /// </summary>
        ///<param name="filters">>An instance of the <see cref="Filters".</param>
        ///<param name="currentOwnerId">Id of current owner.</param>
        /// <returns>Total number of dogs in the system returned after filtering.</returns>
        int GetFilteredDogsCount(Filters filters, Guid currentOwnerId);

        /// <summary>
        /// Deletes a dog from the database.
        /// </summary>
        /// <param name="id">Id of the dog to be deleted.</param>
        void DeleteDog(Guid id);

        /// <summary>
        /// Gets liked dogs of current owner.
        /// </summary>
        /// <param name="page">Page number.</param>
        ///<param name="currentOwnerId">Id of current owner.</param>
        /// <returns>Returns a list of liked dogs as an instance of the class <see cref="DogForDisplay"/>.</returns>
        List<DogForDisplay> GetLikedDogs(int page, Guid currentOwnerId);

        /// <summary>
        ///  Finds the total number of liked dogs of current owner.
        /// </summary>
        /// <returns>Total number of liked dogs of current owner.</returns>
        ///<param name="currentOwnerId">Id of current owner.</param>
        int GetLikedDogsCount(Guid currentOwnerId);

        /// <summary>
        ///  Adds dog and current owner to bridge table of liked dogs.
        /// </summary>
        ///<param name="currentOwnerId">Id of current owner.</param>
        /// <param name="dogId">Id of the dog.</param>
        void LikeDog(Guid currentOwnerId, Guid dogId);
    }
}
