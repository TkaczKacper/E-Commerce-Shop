using System;

namespace ShopAPI.Helpers.Exceptions;

public class UnprocessableContentException(string message) : Exception(message);