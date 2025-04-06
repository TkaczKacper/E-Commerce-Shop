using System;

namespace ShopAPI.Helpers.Exceptions;

public class ConflictException(string message) : Exception(message);