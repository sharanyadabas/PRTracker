import NavigationBar from "../components/NavigationBar";
import Box from "@mui/material/Box";

function PageContainer({ children }) {
  return (
    <Box>
      <NavigationBar />
      {children}
    </Box>
  );
}

export default PageContainer;
