﻿using FastCommerce.Business.DTOs.Product;
using FastCommerce.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FastCommerce.Business.ProductManager.Abstract
{
    public interface IProductManager
    {
        Task<bool> CreateIndexes(ProductElasticIndexDto productElasticIndexDto);
        Task<List<ProductGetDTO>> Get();
        Task<List<GetTrendingProductsDto>> GetTrendingProducts();
        Task<List<TrendingProduct>> GetTrendingProductEntities();
        Task<bool> AddTrendingProduct(TrendingProduct trendingProduct);
        Task<bool> UpdateTrendingProduct(TrendingProduct trendingProduct);
        Task<bool> RemoveTrendingProduct(TrendingProduct trendingProduct);
        Task<ProductGetDTO> GetProductById(int ProdcutId);
        Task<List<ProductGetDTO>> GetProductsByCategoryId(int id);
        Task<List<ProductGetDTO>> GetProductsByCategoryName(string name);
        Task<bool> AddProduct(AddProductDto product);
        Task<List<ProductElasticIndexDto>> SuggestProductSearchAsync(string searchText, int skipItemCount = 0, int maxItemCount = 5);
    }
}
