import { AppBar, Box, Switch, Toolbar, Typography } from "@mui/material";

interface Props {
    darkMode: boolean;
    handleMode: () => void;
}

export default function Header({ darkMode, handleMode }: Props) {
    return (
        <AppBar position="static" sx={{ mb: 4 }}>
            <Toolbar>
            <Box display='flex' alignItems='center'>
                    <Typography variant="h6">
                        RE-STORE
                    </Typography>
                    <Switch checked={darkMode} onChange={handleMode} />
                </Box>
            </Toolbar>
        </AppBar>
    )
}