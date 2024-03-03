namespace SMSHub.APIs.Extensions
{
    public static class AddSwaggerExtension
    {
        //return kestrel after adding middlewares
        //3yza acall through app>>of type webapplication
        public static WebApplication UseSwaggerMiddlewares (this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
           
            return app;
        }
    }
}
