﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using NHibernate;

namespace CIS.UI.Features.Commons.Configurations
{
    public class ImageScaleFactorConfigurationDataInitializer : IDataInitializer
    {
        private readonly ISessionFactory _sessionFactory;

        public ImageScaleFactorConfigurationDataInitializer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Execute()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var imageScaleFactor = session.QueryOver<ImageScaleFactorConfiguration>().Cacheable().SingleOrDefault();
                if (imageScaleFactor == null)
                {
                    imageScaleFactor = new ImageScaleFactorConfiguration();
                    session.Save(imageScaleFactor);
                }

                App.Data.Image = imageScaleFactor;

                transaction.Commit();
            }
        }
    }
}