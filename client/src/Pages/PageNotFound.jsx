

const PageNotFound = () => {
  return (
    <div className="not-found-page"> 
      <div className="container">
        <h1 className="heading">404</h1>
        <h2 className="sub-heading">Page Not Found</h2>
        <p>Oops! The page you are looking for might have been removed or is temporarily unavailable.</p>
        <a href="/" className="button">Go Home</a>
      </div>
    </div>
  );
};

export default PageNotFound;
