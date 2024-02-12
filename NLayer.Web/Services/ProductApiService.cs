﻿using NLayer.Core.DTOs;
using System.Collections.Generic;

namespace NLayer.Web.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;

        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductWithCategoryDTO>> GetProductsWithCategoryAsync()
        {
            //dierkt api'dan dönen datayı json olarak alıyor.
            var response = await _httpClient.GetFromJsonAsync<ApiResponseDTO<List<ProductWithCategoryDTO>>>("product/GetProductsWithCategory");
            return response.Data;
        }
        public async Task<ProductDTO> SaveAsync(ProductDTO product)
        {
            var response = await _httpClient.PostAsJsonAsync("product", product);

            if (!response.IsSuccessStatusCode) return null;

            var responseBody = await response.Content.ReadFromJsonAsync<ApiResponseDTO<ProductDTO>>();

            return responseBody.Data;
        }

        public async Task<bool> UpdateAsync(ProductDTO product)
        {
            var response = await _httpClient.PutAsJsonAsync("product", product);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"product/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponseDTO<ProductDTO>>($"product/{id}");
            return response.Data;
        }
    }
}
