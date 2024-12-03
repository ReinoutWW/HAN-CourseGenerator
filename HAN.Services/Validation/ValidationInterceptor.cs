using System.Reflection;
using Castle.DynamicProxy;
using HAN.Services.Attributes;

namespace HAN.Services.Validation;

/// <summary>
/// Interceptor that performs validation on method arguments when the method is decorated with the <see cref="ValidateEntitiesAttribute"/>
/// Using: DataAnnotations
/// </summary>
public class ValidationInterceptor(IValidationService validationService) : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        var methodImplementation = invocation.MethodInvocationTarget ?? invocation.Method;

        if (MethodHasValidateAttribute(methodImplementation))
        {
            ValidateArguments(invocation);
        }

        invocation.Proceed();
    }

    private static bool MethodHasValidateAttribute(MethodInfo methodImplementation) =>
        Attribute.IsDefined(methodImplementation, typeof(ValidateEntitiesAttribute));

    private void ValidateArguments(IInvocation invocation)
    {
        invocation.Arguments.ToList()
            .ForEach(argument => validationService.Validate(argument));
    }
}