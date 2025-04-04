﻿using System;
using System.ComponentModel.DataAnnotations;
using ShopAPI.Models.Enums;

namespace ShopAPI.Models;

public class Negotiation
{
    [Key]
    public int Id { get; set; }
    
    public int ProductId { get; set; }
    
    public decimal ProposePrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int AttemptCount { get; set; }
    public NegotiationStatus Status { get; set; }
}