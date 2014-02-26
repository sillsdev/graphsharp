namespace GraphSharp.Algorithms
{
    public static class FactoryHelper
    {
        public static TParam CreateNewParameter<TParam>(this IAlgorithmParameters oldParameters)
            where TParam : class, IAlgorithmParameters, new()
        {
            return !(oldParameters is TParam) 
                ? new TParam() : 
                (TParam)(oldParameters as TParam).Clone();
        }
    }
}
