namespace WebAPI
{
    public struct Constants
    {

        public struct RouteConstants
        {
            public struct RestaurantRoute
            {
                public const string BaseRoute = "api/v{version:apiVersion}/[controller]";
                public const string GetRestaurants = "all";
                public const string GetRestaurantsDetails = "details";
                public const string GetRestaurantById = "getRestaurantById";
                public const string CreateRestaurants = "createRestaurant";
                public const string GetRestaurantMenu = "getMenuByRestaurantId";
                public const string CreateMenuCategory = "createRestaurantMenuCategory";
                public const string UpdateRestaurantTitle = "updateRestaurantTitle";
                public const string UpdateRestaurantMenuTitle = "updateRestaurantMenuTitle";
            }

            public struct RestaurantAddressRoute
            {
                public const string BaseRoute = "api/v{version:apiVersion}/[controller]";
                public const string CreateRestaurantAddress = "createRestaurantAddress";
            }

            public struct MenuRoute
            {
                public const string BaseRoute = "api/v{version:apiVersion}/[controller]";
                public const string CreateMenu = "createRestaurantMenu";
                public const string GetRestaurantMenu = "getMenuByMenuId";
                public const string UpdateMenuTitle = "updateMenuTitle";

            }

            public struct MenuCategoryRoute
            {
                public const string BaseRoute = "api/v{version:apiVersion}/[controller]";
                public const string CreateMenuCategory = "createRestaurantMenuCategory";
                public const string CreateCuisine = "createCuisine";
            }

            public struct Identity
            {
                public const string BaseRoute = "api/v{version:apiVersion}/[controller]";
                public const string Registration = "register";
                public const string RestuarantRegistration = "registerAsRestaurantOwner";
                public const string Login = "login";
            }
        }
    }
}
