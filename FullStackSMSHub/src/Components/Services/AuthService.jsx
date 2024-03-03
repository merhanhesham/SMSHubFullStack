const authService = {
    register: async (email, displayName, phoneNumber, password, role) => { // Added role parameter
        try {
            const response = await fetch('https://localhost:7012/api/Accounts/Register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                // Include role in the request body
                body: JSON.stringify({ email, displayName, phoneNumber, password, role }),
            });

            if (!response.ok) {
                // If the response is not okay, attempt to parse the error message
                const errorData = await response.json();
                // Throw an error with both the status code and message for precise error handling
                throw new Error(JSON.stringify({ statusCode: errorData.statusCode, message: errorData.message }));
            }

            const data = await response.json();
            return data;
        } catch (error) {
            console.error('Registration Error:', error);
            // Rethrow the error to be handled by the caller
            throw error;
        }
    },
};

export default authService;
