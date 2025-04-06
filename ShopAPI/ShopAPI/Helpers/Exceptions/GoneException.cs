using System;

namespace ShopAPI.Helpers.Exceptions;

public class GoneException(string message) : Exception(message);
    