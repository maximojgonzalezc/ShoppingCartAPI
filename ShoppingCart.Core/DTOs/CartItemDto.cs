﻿namespace ShoppingCart.Core.DTOs;

public class CartItemDto
{
    public ProductDto? Product { get; set; }
    public int Quantity { get; set; }
}
