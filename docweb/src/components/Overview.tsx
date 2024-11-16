import { Typography, Box, Link } from "@mui/material";

interface Props {
  overview: string | undefined;
}

const Overview = ({ overview }: Props): JSX.Element => {
  return (
    <Box
      display="flex"
      flexDirection="column"
      justifyContent="center"
      alignItems="center"
      height="100%"
      textAlign="center"
    >
      <Typography variant="h4" fontWeight="bold">
        Repository Documentation Overview
      </Typography>
      <Typography
        variant="body2"
        color="textSecondary"
        sx={{
          mt: 5,
          maxWidth: "400px",
        }}
      >
        {overview}
      </Typography>
      <Typography variant="body1" color="textSecondary" sx={{ mt: 5 }}>
        To start browsing, click "Files" on the left, or explore your{" "}
        <Link href="/codecity">Code City</Link>.
      </Typography>
    </Box>
  );
};

export default Overview;
