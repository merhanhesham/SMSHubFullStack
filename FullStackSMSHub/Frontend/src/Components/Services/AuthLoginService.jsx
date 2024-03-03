
const authService = {
    login: async (email, password) => {
        try {
            const response = await fetch('https://localhost:7012/api/Accounts/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ email, password }),
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Login failed');
            }

            const { token } = await response.json();
            localStorage.setItem('authToken', token);

            return token;
        } catch (error) {
            console.error('Login Error:', error);
            throw error;
        }
    },
};

export default authService;
