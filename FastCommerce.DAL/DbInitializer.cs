﻿using FastCommerce.Entities.Entities;
using GenFu;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace FastCommerce.DAL
{
    public static class DbInitializer
    {
        private static List<Product> GetFakeProductData(int count)
        {
            var i = 1;
            var results = A.ListOf<Product>(count);
            results.ForEach(x => x.ProductId = i++);
            return results.Select(_ => _).ToList();
        }


        private static List<T> FillAllProperties<T>(int count) where T : new()
        {
            var results = A.ListOf<T>(count);
            PropertyInfo[] properties = typeof(T).GetProperties();
            PropertyInfo PKInfo = null;
            PropertyInfo FKInfo;
            int Pk = 0;
            foreach (var prop in properties)
            {
                List<Attribute> attrs = prop.GetCustomAttributes().ToList();
                foreach (var obj in attrs)
                {
                    if (obj.GetType() == typeof(ForeignKey))
                    {
                        FKInfo = prop;
                        break;
                    }
                    if (obj.GetType() == typeof(KeyAttribute))
                    {
                        PKInfo = prop;
                        break;
                    }
                }
            }
            foreach (T row in results)
            {
                row.GetType().GetProperty(PKInfo.Name).SetValue(row,0);
            }
            return results.ToList();
        }

        public async static void Initialize(dbContext context)
        {
            
            context.Database.EnsureCreated();
            if (context.Products.Any())
                return;

            int i = 0;
            int count = 5;
            int countSquare = (int) Math.Pow(count, 2);
            List<Product> products = FillAllProperties<Product>(5);

            List<Category> categories = FillAllProperties<Category>(5);

            List<CategoryProperties> categoryProperties = FillAllProperties<CategoryProperties>(countSquare);

            List<ProductCategories> productCategories = FillAllProperties<ProductCategories>(countSquare);

            List<Entities.Entities.Property> properties = FillAllProperties<Entities.Entities.Property>(5);
            
            //foreach (var item in categories)
            //{
            //    item.Properties = properties.ToList();
            //}

            for (var j = 0; j < countSquare; j++)
            {
                productCategories[j].ProductId = products.ToList()[j / count].ProductId;
                productCategories[j].Product = products.ToList()[j/count];
                productCategories[j].CategoryId = categories.ToList()[j % 5].CategoryId;
                productCategories[j].Category = categories.ToList()[j%5];
            }
            for (var j = 0; j < countSquare; j++)
            {
                categoryProperties[j].CategoryId = categories.ToList()[j / count].CategoryId;
                categoryProperties[j].Category = categories.ToList()[j / count];
                categoryProperties[j].PropertyId = properties.ToList()[j % 5].PropertyID;
                categoryProperties[j].Property = properties.ToList()[j % 5];
            }

            //foreach (var item in categories)
            //{
            //    item.ProductCategoriesId = productCategories.Single(pc => pc.CategoryId == item.CategoryId)
            //        .ProductCategoriesId;
            //    item.CategoryPropertiesId = categoryProperties.Single(cp => cp.CategoryId == item.CategoryId)
            //        .CategoryPropertiesId;
            //}
            List<StockProperties> stockProperties = FillAllProperties<StockProperties>(5);
            List<Stock> stocks = FillAllProperties<Stock>(5);
            i = 0;
            foreach (var item in stocks)
            {
                item.Product = products.ToList()[i];
                i++;

            }

            i = 0;
            foreach (var item in stockProperties)
            {
                item.Stock = stocks.ToList()[i];
                item.Property = properties.ToList()[i];
                i++;
            }

            foreach (var item in properties)
            {
                item.StockProperties = stockProperties.ToList();
            }


            await context.AddRangeAsync(FillAllProperties<User>(5));

            await context.AddRangeAsync(categoryProperties);
            await context.AddRangeAsync(productCategories);
            await context.AddRangeAsync(products);
            await context.AddRangeAsync(properties);
            await context.AddRangeAsync(categories);
            await context.AddRangeAsync(stockProperties);
            await context.AddRangeAsync(stocks);
            context.SaveChanges();
        }
    }
}
