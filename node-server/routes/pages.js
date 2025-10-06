app.get("/users", async (req, res) => {
    try {
        const response = await axios.get("http://localhost:3000/api/users"); // <- calling your own API
        const users = response.data;

        res.render("users", {
            title: "Users",
            users: users
        });
    } catch (err) {
        res.status(500).send("Failed to load users page.");
    }
});
