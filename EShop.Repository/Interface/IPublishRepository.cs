﻿using EShop.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Repository.Interface
{
    public interface IPublishRepository
    {
        List<Publishers> GetAllPublishers();
        Publishers GetDetailsForRent(Guid? id);
    }
}
