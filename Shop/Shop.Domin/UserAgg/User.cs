using Common.Domain;
using Common.Domain.Exceptions;
using Shop.Domain.UserAgg.Enums;
using Shop.Domain.UserAgg.Service;
using System.Globalization;

namespace Shop.Domain.UserAgg
{
    public class User : AggregateRoot
    {
        public User(string name, string family, string phoneNumber, string email,
            string password, Gender gender, IUserDomainService userDomainService)
        {
            Guard(phoneNumber, email, userDomainService);

            Name = name;
            Family = family;
            PhoneNumber = phoneNumber;
            Email = email;
            Password = password;
            Gender = gender;
            AvatarName = "avatar.png";
            IsActive = true;
            Roles = new();
            Wallets = new();
            Addresses = new();
        }


        public string Name { get; private set; }
        public string Family { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string AvatarName { get; set; }
        public bool IsActive { get; set; }

        public Gender Gender { get; private set; }
        public List<UserRole> Roles { get; }
        public List<Wallet> Wallets { get; }
        public List<UserAddress> Addresses { get; }

        public void Edit(string name, string family, 
            string phoneNumber, string email, Gender gender ,IUserDomainService userDomainService)
        {
            Guard(phoneNumber, email, userDomainService);
            Name = name;
            Family = family;
            PhoneNumber = phoneNumber;
            Email = email;
            Gender = gender;
        }
        public static User RegisterUser(string phoneNumber, string password, IUserDomainService userDomainService)
        {
            return new User("", "", phoneNumber, null, password, Gender.None, userDomainService);
        }

        public void AddAddress(UserAddress address)
        {
            address.UserId = Id;
            Addresses.Add(address);
        }
        public void DeleteAddress(long addressId)
        {
            var oldAddress = Addresses.FirstOrDefault(a => a.Id == addressId);
            if (oldAddress == null) throw new NullOrEmptyDomainDataException("Address Not Found");
            Addresses.Remove(oldAddress);
        }

        public void EditAddress(UserAddress address)
        {
            var oldAddress = Addresses.FirstOrDefault(a => a.Id == address.Id);
            if (oldAddress == null) throw new NullOrEmptyDomainDataException("Address Not Found");
            Addresses.Remove(oldAddress);
            Addresses.Add(address);
        }
        public void ChargeWallet(Wallet wallet)
        {
            Wallets.Add(wallet);
        }

        public void SetRole(List<UserRole> roles)
        {
            Roles.Clear();
            Roles.AddRange(roles);
        }

        public void Guard(string phoneNumber, string email, IUserDomainService domainService)
        {
            NullOrEmptyDomainDataException.CheckString(phoneNumber, nameof(phoneNumber));
            NullOrEmptyDomainDataException.CheckString(email, nameof(email));

            if (phoneNumber.Length != 11) throw new InvalidDomainDataException("Phone Number Isn't Valid");
            if (email.IsValidEmail() == false) throw new InvalidDomainDataException("Email Isn't Valid");

            if (phoneNumber != PhoneNumber)
                if (domainService.IsPhoneNumberExist(phoneNumber))
                    throw new InvalidDomainDataException("This PhoneNumber Is Exist Please Try Another One");


            if (email != Email)
                if (domainService.IsEmailExist(email))
                    throw new InvalidDomainDataException("This Email Is Exist Please Try Another One");

        }
    }

}
